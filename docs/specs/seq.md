# Sequences (.bfseq, .bcseq)
Sequences play notes from a particular bank. This can be used for sound effects, level music, or anything else.

## The Main File
The main file consists of a standard File Header, a Data Block, and a Label block. Each block is padded to be 0x20 bytes. The file data is the same for each version, with the standard being 1.0.0.

| **Type** | **Description** |
|----------|-----------------|
|FileHeader|Standard File Header (Magic: FSEQ or CSEQ. Always contains 2 blocks)|
|Block|Data Block (Reference Type: 0x5000)|
|Block|Label Block (Reference Type: 0x5001)|

## Data Block Body (Magic: DATA)
Contains only raw sequence data. See the [Sequence Data Specification](specs/seqData.md) for details. Padded to 0x20 bytes. It's important to note that the sequence data is always big endian for the Wii U and 3ds, but little endian for the Switch.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|u8[Block size - 8]|Raw sequence data|
|----|---|Padding for alignment|

## Label Block Body (Magic: LABL)
Contains labels to jump to in the sequence data.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Table`<Reference`>|References to each Label (Reference Type: 0x5100)|
|0x04 + 8 * Table Count|Label[Table Count]|Labels referenced earlier|
|----|---|Padding for alignment|

### Label
Contains the label name and offset to data in the sequence data. Think of a label as an entrypoint in the data, like a door. Each label is padded to 0x4 bytes.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Reference|Reference to start in raw sequence data (Reference Type: 0x1F00)|
|0x08|u32|Size of the label name|
|0x0C|char[Size of label name]|The name of the label. There is no null terminator, since it's a char array|

## Summary
A tree to sum up the structure of the file:
```
SEQ File:
|-File Header
|
|-Data Block
|    |-Raw sequence data.
|
|-Label Block
|    |-Reference table to labels.
|        |-Label with reference to data entrypoint, and name.
|
|-Files
```