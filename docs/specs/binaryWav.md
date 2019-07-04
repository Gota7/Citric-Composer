# Binary Wave Files (.bwav)
BWAVs, or Binary Waves are a new addition to the AAL library since SMM2. They are used how STMs are, but are in a structure that resembles a WAV. Since these are part of the AAL library and not NintendoWare, these do not follow the same common structures as other files. In fact, these files actually use absolute offsets.

## The Main File
The main file consists of a File Header, an array of Channel Info, and an array of Channel Samples. There is padding to 0x40 before each structure.

| **Type** | **Description** |
|----------|-----------------|
|FileHeader|File Header (Magic: BWAV)|
|ChannelInfo[NumChannels]|Channel Info|
|Padding|Padding to 0x40|
|ChannelSamples[NumChannels]|Array of Channel Samples. There is a padding of 0x40 before each channel's samples|

## File Header
A really different header than most files. It is 0x10 bytes.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|char[4]|Magic|
|0x04|u16|Byte Order. 0xFFFE in big endian is little endian|
|0x06|u16|Version. &0xFF00 >> 8 is Major, &0xFF is Minor|
|0x08|u32|CRC32 Hash. Combine all channels' sample data into one large byte array without padding, and CRC32 hash it to get this|
|0x0C|u16|Padding|
|0x0E|u16|Num Channels. How many channels this file contains|

## Channel Info
Gives the game info about how to play the audio of a certain channel. It's 0x4C bytes.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|u16|Always seems to be 1|
|0x02|u16|Channel Pan. 0 for left, 1 for right, 2 for middle|
|0x04|u32|Sample Rate|
|0x08|u32|Number of samples|
|0x0C|u32|Number of samples again?|
|0x10|s16[8][2]|DSP-ADPCM Coefficients|
|0x30|u32|Absolute start offset of the sample data|
|0x34|u32|Absolute start offset of the sample data again?|
|0x38|u32|Is 1 if the channel loops|
|0x3C|u32|Loop End Sample|
|0x40|u32|Loop Start Sample|
|0x44|u16|Predictor Scale?|
|0x46|u16|History Sample 1?|
|0x48|u16|History Sample 2?|
|0x4A|u16|Padding?|

## Channel Samples
Each channel has its data stored using DSP-ADPCM encoding. The length of the the byte array of each channel is the length of the entire file minus the offset of the last channel. Before each channel's sample array, the file is padded to 0x40 bytes, not at the end though.

## Summary
A tree to sum up the structure of the file:
```
BWAV File:
|-File Header
|
|-Channel Info[NumChannels]
|    |-Information about each channel.
|
|-Channel Samples
```