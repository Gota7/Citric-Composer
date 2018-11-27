using System;
using System.Collections.Generic;
using System.IO;
using Syroot.BinaryData;
using static CitraFileLoader.b_stm;

namespace CitraFileLoader
{
    
    /// <summary>
    /// Stream Prefetch File.
    /// </summary>
	public class b_stp {

        /// <summary>
        /// 1 - File header.
        /// </summary>
		public FileHeader fileHeader;

        /// <summary>
        /// 2 - Info block.
        /// </summary>
		public b_stm.InfoBlock info;

        /// <summary>
        /// 3 - Prefetch data.
        /// </summary>
		public PdatBlock pdat;


        /// <summary>
        /// Prefetch data block.
        /// </summary>
		public class PdatBlock {

            /// <summary>
            /// 1 - PDAT.
            /// </summary>
			public char[] magic;

            /// <summary>
            /// 2 - Block size.
            /// </summary>
			public UInt32 size;

            /// <summary>
            /// 3 - Prefetch data table.
            /// </summary>
			public Table<PrefetchData> prefetchData;

			/// <summary>
            /// 4 - DSP-ADPCM samples. [NumPrefetch][NumChannels][PrefetchSize]
            /// </summary>
            public byte[][][] samples;


            /// <summary>
            /// Prefetch data.
            /// </summary>
			public class PrefetchData {

                /// <summary>
                /// 1 - Starting frame.
                /// </summary>
				public UInt32 startFrame;

                /// <summary>
                /// 2 - Size of prefetch data.
                /// </summary>
				public UInt32 prefetchSize;

                /// <summary>
                /// 3 - Reserved.
                /// </summary>
				public UInt32 reserved;

                /// <summary>
                /// 4 - Reference to prefetch sample.
                /// </summary>
				public Reference toPrefetchSample;

			}

            /// <summary>
            /// Get the size of the block.
            /// </summary>
            /// <returns>The size.</returns>
			public UInt32 GetSize() {

                //Magic, size, and table count.
				UInt32 size = 12;
				size += prefetchData.count * 20;

                //Return size.
				return size;

			}

		}

        
        /// <summary>
        /// Load a file.
        /// </summary>
        /// <param name="b">The blue component.</param>
		public void Load(byte[] b) {

			//The reader.
			MemoryStream src = new MemoryStream(b);
			BinaryDataReader br = new BinaryDataReader(src);

			//Read the header.
            fileHeader = new FileHeader(ref br);

            //Read blocks.
            foreach (SizedReference s in fileHeader.blockOffsets)
            {

                if (s.offset != Reference.NULL_PTR)
                {
                    br.Position = s.offset;
                    switch (s.typeId)
                    {

                        //Info block.
                        case ReferenceTypes.STRM_Block_Info:
                            long basePos = br.Position + 8;
                            info = new InfoBlock()
                            {
                                magic = br.ReadChars(4),
                                blockSize = br.ReadUInt32(),
                                streamSoundInfoRef = new Reference(ref br),
                                trackInfoTableRef = new Reference(ref br),
                                channelInfoTableRef = new Reference(ref br),
                                streamSoundInfo = null,
                                trackInfoRefTable = null,
                                channelInfoRefTable = null,
                                tracks = null,
                                channels = null
                            };

                            //Stream sound info.
                            if (info.streamSoundInfoRef.typeId == ReferenceTypes.STRM_Info_StreamSound && info.streamSoundInfoRef.offset != Reference.NULL_PTR)
                            {

                                br.Position = basePos + info.streamSoundInfoRef.offset;
                                info.streamSoundInfo = new StreamSoundInfo()
                                {
                                    encoding = br.ReadByte(),
                                    isLoop = br.ReadBoolean(),
                                    channelCount = br.ReadByte(),
                                    regionCount = br.ReadByte(),
                                    sampleRate = br.ReadUInt32(),
                                    loopStart = br.ReadUInt32(),
                                    sampleCount = br.ReadUInt32(),
                                    blockCount = br.ReadUInt32(),
                                    oneBlockBytesize = br.ReadUInt32(),
                                    oneBlockSamples = br.ReadUInt32(),
                                    lastBlockBytesize = br.ReadUInt32(),
                                    lastBlockSamples = br.ReadUInt32(),
                                    lastBlockPaddedBytesize = br.ReadUInt32(),
                                    sizeOfSeekInfo = br.ReadUInt32(),
                                    seekInfoIntervalSamples = br.ReadUInt32(),
                                    sampleDataOffset = new Reference(ref br),
                                    regionInfoBytesize = 0x100,
                                    padding = 0,
                                    regionDataOffset = new Reference(0, 0x18),
                                    originalLoopStart = 0,
                                    originalLoopEnd = 0,
                                    secretInfo = 0
                                };

                                if (fileHeader.vMajor >= regionInfo)
                                {
                                    info.streamSoundInfo.regionInfoBytesize = br.ReadUInt16();
                                    info.streamSoundInfo.padding = br.ReadUInt16();
                                    info.streamSoundInfo.regionDataOffset = new Reference(ref br);
                                }

                                if (fileHeader.vMajor >= originalLoopInfo)
                                {
                                    info.streamSoundInfo.originalLoopStart = br.ReadUInt32();
                                    info.streamSoundInfo.originalLoopEnd = br.ReadUInt32();
                                }

                                if (fileHeader.vMajor >= secretInfo)
                                {
                                    info.streamSoundInfo.secretInfo = br.ReadUInt32();
                                }

                            }

                            //Track info.
                            if (info.trackInfoTableRef.typeId == ReferenceTypes.Tables + 1 && info.trackInfoTableRef.offset != Reference.NULL_PTR)
                            {

                                br.Position = basePos + info.trackInfoTableRef.offset;
                                long newPos = br.Position;
                                info.trackInfoRefTable = new ReferenceTable(ref br);

                                //Get tracks.
                                info.tracks = new List<TrackInfo>();
                                foreach (Reference r in info.trackInfoRefTable.references)
                                {

                                    TrackInfo t = null;
                                    if (r.typeId == ReferenceTypes.STRM_Info_Track && r.offset != Reference.NULL_PTR)
                                    {

                                        br.Position = newPos + r.offset;
                                        t = new TrackInfo()
                                        {
                                            volume = br.ReadByte(),
                                            pan = br.ReadByte(),
                                            span = br.ReadByte(),
                                            surroundMode = br.ReadByte(),
                                            globalChannelIndexTableRef = new Reference(ref br),
                                            globalChannelIndexTable = null
                                        };

                                        if (t.globalChannelIndexTableRef.offset != Reference.NULL_PTR)
                                        {

                                            br.Position = newPos + r.offset + t.globalChannelIndexTableRef.offset;
                                            t.globalChannelIndexTable = new Table<byte>();
                                            t.globalChannelIndexTable.count = br.ReadUInt32();
                                            t.globalChannelIndexTable.entries = new List<byte>();
                                            for (int i = 0; i < t.globalChannelIndexTable.count; i++)
                                            {
                                                t.globalChannelIndexTable.entries.Add(br.ReadByte());
                                            }

                                        }

                                    }
                                    info.tracks.Add(t);

                                }

                            }

                            //Channel info.
                            if (info.channelInfoTableRef.typeId == ReferenceTypes.Tables + 1 && info.channelInfoTableRef.offset != Reference.NULL_PTR)
                            {

                                br.Position = basePos + info.channelInfoTableRef.offset;
                                long newPos = br.Position;
                                info.channelInfoRefTable = new ReferenceTable(ref br);

                                //Get channels.
                                info.channels = new List<ChannelInfo>();
                                foreach (Reference r in info.channelInfoRefTable.references)
                                {

                                    ChannelInfo c = null;
                                    if (r.offset != Reference.NULL_PTR)
                                    {

                                        br.Position = newPos + r.offset;
                                        c = new ChannelInfo()
                                        {
                                            dspAdpcmInfoRef = new Reference(ref br),
                                            dspAdpcmInfo = null
                                        };

                                        if (c.dspAdpcmInfoRef.offset != Reference.NULL_PTR)
                                        {
                                            br.Position = newPos + r.offset + c.dspAdpcmInfoRef.offset;
                                            c.dspAdpcmInfo = new DspAdpcmInfo();
                                            c.dspAdpcmInfo = new DspAdpcmInfo() { coefs = new short[8][] };
                                            c.dspAdpcmInfo.coefs[0] = br.ReadInt16s(2);
                                            c.dspAdpcmInfo.coefs[1] = br.ReadInt16s(2);
                                            c.dspAdpcmInfo.coefs[2] = br.ReadInt16s(2);
                                            c.dspAdpcmInfo.coefs[3] = br.ReadInt16s(2);
                                            c.dspAdpcmInfo.coefs[4] = br.ReadInt16s(2);
                                            c.dspAdpcmInfo.coefs[5] = br.ReadInt16s(2);
                                            c.dspAdpcmInfo.coefs[6] = br.ReadInt16s(2);
                                            c.dspAdpcmInfo.coefs[7] = br.ReadInt16s(2);
                                            c.dspAdpcmInfo.pred_scale = br.ReadUInt16();
                                            c.dspAdpcmInfo.yn1 = br.ReadInt16();
                                            c.dspAdpcmInfo.yn2 = br.ReadInt16();
                                            c.dspAdpcmInfo.loop_pred_scale = br.ReadUInt16();
                                            c.dspAdpcmInfo.loop_yn1 = br.ReadInt16();
                                            c.dspAdpcmInfo.loop_yn2 = br.ReadInt16();
                                        }

                                    }
                                    info.channels.Add(c);

                                }

                            }

                            break;

                        //Data block.
                        case ReferenceTypes.STRM_Block_PrefetchData:
							pdat = new PdatBlock();
							pdat.magic = br.ReadChars(4);
							pdat.size = br.ReadUInt32();
                            break;

                    }
                }

            }

		}


        /// <summary>
        /// Update the file.
        /// </summary>
        /// <param name="endian">Endian.</param>
		public void Update(UInt16 endian) {

			//Update info.
            info.magic = "INFO".ToCharArray();
            info.streamSoundInfoRef = new Reference(ReferenceTypes.STRM_Info_StreamSound, 0x18);

            //Stream sound info.
            UInt32 infoBaseOffet = 0x18;

            info.streamSoundInfo.channelCount = (byte)info.channels.Count;
			info.streamSoundInfo.regionCount = 0;
            info.streamSoundInfo.regionDataOffset = new Reference(0, Reference.NULL_PTR);
            info.streamSoundInfo.regionInfoBytesize = 0x100;
            info.streamSoundInfo.sampleDataOffset = new Reference(ReferenceTypes.General_ByteStream, 0x18);
            info.streamSoundInfo.padding = 0;
            info.streamSoundInfo.sizeOfSeekInfo = 4;

            infoBaseOffet += 56;
            if (fileHeader.vMajor >= b_stm.regionInfo) infoBaseOffet += 12;
			if (fileHeader.vMajor >= b_stm.originalLoopInfo) infoBaseOffet += 8;
			if (fileHeader.vMajor >= b_stm.secretInfo) infoBaseOffet += 4;

            //Reference table for track info ref.
            info.trackInfoTableRef = new Reference(0x0101, (int)infoBaseOffet);

            //Skip ref table coords.
            infoBaseOffet += (uint)(info.tracks != null ? (info.tracks.Count * 8 + 4) : 0);

            info.channelInfoTableRef = new Reference(0x0101, (int)infoBaseOffet);

            List<Reference> trackRefs = new List<Reference>();

            //Track info.
            UInt32 trackBaseOffset = (uint)((info.tracks != null ? (info.tracks.Count * 8 + 4) : 0) + (info.channels.Count * 8 + 4));
            if (info.tracks != null)
            {
                for (int i = 0; i < info.tracks.Count; i++)
                {

                    trackRefs.Add(new Reference(ReferenceTypes.STRM_Info_Track, (int)trackBaseOffset));

                    infoBaseOffet += 12;
                    trackBaseOffset += 12;
                    if (info.tracks[i].globalChannelIndexTable != null)
                    {
                        info.tracks[i].globalChannelIndexTableRef = new Reference(0x100, 0xC);
                        info.tracks[i].globalChannelIndexTable.count = (uint)info.tracks[i].globalChannelIndexTable.entries.Count;
                        infoBaseOffet += 4 + info.tracks[i].globalChannelIndexTable.count;
                        trackBaseOffset += 4 + info.tracks[i].globalChannelIndexTable.count;
                        while (infoBaseOffet % 4 != 0)
                        {
                            infoBaseOffet++;
                            trackBaseOffset++;
                        }
                    }
                    else
                    {
                        info.tracks[i].globalChannelIndexTableRef = new Reference(0, Reference.NULL_PTR);
                    }

                }
                info.trackInfoRefTable = new ReferenceTable(trackRefs);
            }
            else
            {
                info.trackInfoTableRef = new Reference(0, Reference.NULL_PTR);
            }

            //Channel info.
            UInt32 channelBaseOffset = (uint)(4 + 8 * info.channels.Count + trackBaseOffset - ((uint)((info.tracks != null ? (info.tracks.Count * 8 + 4) : 0) + (info.channels.Count * 8 + 4))));
			infoBaseOffet += (uint)(4 + 8 * info.channels.Count);
            List<Reference> channelRefs = new List<Reference>();

            for (int i = 0; i < info.channels.Count; i++)
            {

                channelRefs.Add(new Reference(ReferenceTypes.STRM_Info_Channel, (int)channelBaseOffset));
                channelBaseOffset += 8;
                infoBaseOffet += 8;

            }

            info.channelInfoRefTable = new ReferenceTable(channelRefs);

			channelBaseOffset = (uint)(8 * info.channels.Count);
            for (int i = 0; i < info.channels.Count; i++)
            {

                info.channels[i].dspAdpcmInfoRef = new Reference(0, Reference.NULL_PTR);
                if (info.channels[i].dspAdpcmInfo != null)
                {
                    info.channels[i].dspAdpcmInfoRef = new Reference(0x300, (int)channelBaseOffset);
                    channelBaseOffset += 0x2E;
                    infoBaseOffet += 0x2E;
                }
                channelBaseOffset -= 8;

            }

			//Update pdat.
			pdat.magic = "PDAT".ToCharArray();
			pdat.size = pdat.GetSize();
			pdat.prefetchData.count = (uint)pdat.prefetchData.entries.Count;
			Int32 offset = 0x34;
			for (int i = 0; i < pdat.prefetchData.count; i++) {
				pdat.prefetchData.entries[i].reserved = 0;
				pdat.prefetchData.entries[i].toPrefetchSample = new Reference(0, offset);
			}

            //Make offsets, depeding on what blocks exist, and on the sizes of the blocks.
            List<SizedReference> blocks = new List<SizedReference>();

            //Can't forget about magic and size.
            infoBaseOffet += 8;

            while (infoBaseOffet % 0x20 != 0)
            {
                infoBaseOffet++;
            }
            info.blockSize = infoBaseOffet;

            blocks.Add(new SizedReference(ReferenceTypes.STRM_Block_Info, 0, info.blockSize));
            blocks.Add(new SizedReference(ReferenceTypes.STRM_Block_PrefetchData, (int)info.blockSize, pdat.GetSize()));
                                       
			fileHeader = new FileHeader(endian == ByteOrder.LittleEndian ? "CSTM" : "FSTM", endian, fileHeader.version, (uint)(info.blockSize + pdat.GetSize()), blocks);
         

		}

        /// <summary>
        /// Convert to bytes.
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="endian">Endian.</param>
		public byte[] ToBytes(UInt16 endian) {
            
            //Update.
			Update(endian);

			//New writer.
			MemoryStream o = new MemoryStream();
			BinaryDataWriter bw = new BinaryDataWriter(o);

			//...

			//Return file.
			return o.ToArray();

		}

	}


    /// <summary>
    /// Prefetch Factory.
    /// </summary>
	public static class PrefetchFactory {


        /// <summary>
        /// Creates a prefetch stream.
        /// </summary>
        /// <returns>The prefetch stream.</returns>
        /// <param name="s">S.</param>
        /// <param name="version">Version.</param>
		public static b_stp CreatePrefetchStream(b_stm s, UInt32 version) {

            //New prefetch data.
			b_stp p = new b_stp();

			//Change version, and update info.
			p.fileHeader = new FileHeader("FSTP", ByteOrder.BigEndian, version, 0, new System.Collections.Generic.List<SizedReference>());
			p.info = s.info;
            
			//Make prefetch data.
			p.pdat = new b_stp.PdatBlock();
			p.pdat.prefetchData = new Table<b_stp.PdatBlock.PrefetchData>();
			p.pdat.prefetchData.entries = new System.Collections.Generic.List<b_stp.PdatBlock.PrefetchData>();
			p.pdat.prefetchData.entries.Add(new b_stp.PdatBlock.PrefetchData()
			{

				startFrame = 0,
				prefetchSize = 0xA000 * (uint)p.info.streamSoundInfo.channelCount
                                               
			});

			//Audio data.
			p.pdat.samples = new byte[1][][];
			p.pdat.samples[0] = s.data.dspAdpcm;

			//Return data.
			return p;

		}

	}

}
