# Common Structures
In every type of sound file, you are bound to come across at least one of these structures. Rather than redefining them for each file, it would be better to define them all here for simplicity.

## Primitive Types
Every structure is made of the following primitive types. You will come across the following:

| **Type** | **Description** |
|----------|-----------------|
|u8|8-bit Unsigned Integer|
|u16|16-bit Unsigned Integer|
|u32|32-bit Unsigned Integer|
|s8|8-bit Signed Integer|
|s16|16-bit Signed Integer|
|s32|32-bit Signed Integer|
|f32|32-bit Floating Point Number|
|string|Since NintendoWare was coded in C++, every string is null terminated and includes '\0' at the end. This null terminator is included in the length of the string|
|char[Array Size]|Char Array. This is like a string, except that it does not have a null terminator, and always has a fixed size|

## References
In order to point to another structure stored in some location, NintendoWare uses references in order to jump to them. All references have offsets, which are relative to the start of the structure. Any type of reference itself does not count as a structure in this sense. For example, if a structure begins at 0x20, and there is a reference within the structure at 0x30, and the offset to another structure is 0x70, then the absolute address of the other structure is 0x90. The reference takes the position of the structure it is in, and adds its offset to it in order to reach the destination. If a reference has an offset of -1, then it is null, and does not point to any data (the data is null).

### Reference
A basic reference to data.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|u16|Type specifier for the data that this reference points to (Usually 0 if null reference, but can be different in special cases)|
|0x02|u16|Padding For Alignment (Always 0)|
|0x04|s32|Relative offset to data (-1 if the reference is null)|

### Sized Reference
It's like a reference, but it also contains the size of the data it points to.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|u16|Type specifier for the data that this reference points to|
|0x02|u16|Padding For Alignment (Usually 0 if null reference, but can be different in special cases)|
|0x04|s32|Relative offset to data (Always 0)|
|0x08|u32|Size of the referenced data (Always 0 if the reference is null)|

## Main Structures
These are the main structures used within the files. Each one counts as its own structure, with the exception of Ids, Wave Ids, BitFlags, and Enumerations.

### File Header
This is essential for the game to know how to read the data in the file. It is always padded to 0x20 bytes.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|char[4]|Magic identifer for the file. The first char specifies the type of file. 'F' for 'F' type, and 'C' for 'C' type|
|0x04|u16|Byte order. 0xFEFF for big endian, and 0xFFFE for little endian. Note, that these values are in big endian|
|0x06|u16|Size of this header|
|0x08|Version|Version of this file|
|0x0C|u32|Size of this file|
|0x10|u16|Number of blocks in the file|
|0x12|u16|Padding for alignment|
|0x14|SizedReference[Number of blocks in the file]|Sized references to the blocks contained in this file|
|----|---|Padding for alignment|

### Version
Weirdly, this is read as a u32, meaning how it's read varies on the endianess of the file. What is also weird is that this is different for 'F' and 'C' type files. For 'F' type files, the form is 0x00MMIIRR, but for 'C' type files, it is 0xMMIIRR00. This is after reading in the proper endian. M is the major version of the file, I is the minor version of the file, and R is the revision version of the file. These values determine how the file is to be read, if the file's format varies due to its version.

### Block
A block contains data for a certain purpose of its file. Every structure except the file header has to be within a block.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|char[4]|Magic identifier for the block|
|0x04|u32|Size of this entire block structure|
|0x08|BlockBody|The block body that contains data specific to the file. Note that this is its own structure, so References within the Block Body add to the absolute address of the start position of the Block Body|

### Id
When referencing an entry in the main Sound Archive, an Id is used to provide both the type of entry, and the index of that entry. An Id is a 32-bit value in the form 0xTTIIIIII, where T is the Sound Type, and I is the index.

### Wave Id
When referencing a wave within a Wave Archive, a special type of id is used.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Id|Id pointing to the Wave Archive entry in the main Sound Archive. The Sound Type is always 5|
|0x04|u32|Index of the wave within the wave archive|

### Sound Types (Enumeration)
A sound type, and its value.

| **Value** | **Sound Type** |
|-----------|----------------|
|0|Null Type|
|1|Sound (Sequence, Stream, Wave Sound Data)|
|2|Sound Group (Sequence Set, Wave Sound Data Set)|
|3|Bank|
|4|Player|
|5|Wave Archive|
|6|Group|

### Table`<Type T`>
This contains a collection of whatever the Type T is. For example, a table could contain u8s, References, or anything else. Tables are always padded to 0x4 bytes.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|u32|Number of entries|
|0x04|T[Number of entries]|An array of Type T, with the number of entries set to the value above|
|----|---|Padding for alignment|

### Bit Flags
These present an efficient way to add optional data to a parameter but only increasing the size when needed. The Bit Flag is a u32 in which every bit specifies a bit that is set. Each set bit represents an additional u32 to read for that corresponding flag. For example, for a Bit Flag value of 0x00000301, that u32 is followed by 3 more u32s. The first one for flag 0, the next for flag 8, and the last one for flag 9. The bit number corresponds with the flag number, and the values appear in that order.

More will be added when needed.