# Wave Archive Files (.bfwar, .bcwar)
WARs, or Wave Archives, are nothing more than a "folder" of bfwav or bcwav files. They are used to house multiple waveforms, be them for SFX, instrument samples, or even music in rare cases. Since these things tend to have a great amount of embedded files, these files are not referenced in the file table for the main Sound Archive. These files are relatively simple, as they don't contain any information other than for storing waveforms.

## The Main File
The main file consists of a standard File Header, an Info Block, and a File Block. Each block is padded to be 0x20 bytes. The file data is the same for each version, with the standard being 1.0.0.

| **Type** | **Description** |
|----------|-----------------|
|FileHeader|Standard File Header (Magic: FWAR or CWAR. Always contains 2 blocks)|
|Block|Info Block (Reference Type: 0x6800)|
|Block|File Block (Reference Type: 0x6801)|

## Info Block Body (Magic: INFO)
This is perhaps the simplest info block you will ever come across. Padded to 0x20 bytes.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Table`<SizedReference>`|References to each Wave File in the File Block (Reference Type: 0x1F00)|
|----|---|Padding for alignment|

## File Block Body (Magic: FILE)
All the files referenced from the Info Block are in here. Each offset is relative to the beggining of this block body. If the reference is null, then it's a null file and no file exists for it. Each file and this block itself are padded to 0x20 bytes, but the padding is not included in the size of the size reference. Since there are no symbols attached to the waves embedded, since they are not referenced as files in the main Sound Archive, b_wavs do not have a filename. 

## Summary
A tree to sum up the structure of the file:
```
WAR File:
|-File Header
|
|-Info Block
|    |-Table of sized references to the files.
|
|-File Block
|    |-Collection of files referenced from the info block.
|
|-Files
```