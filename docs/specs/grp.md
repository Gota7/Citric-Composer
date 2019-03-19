# Groups (bfgrp, bcgrp)
GRPs, or Groups, are perhaps the most confusing part of the Sound Archive. As mentioned in the Sound Archive specification, a file in there can be any one of these types: Undefined, Internal, External, in Group Only, or Informal External. The In Group Only type is an Internal file that has a null sized reference because the file data for that file id is only inside a Group file. Note that this is impossible to distinguish between Informal External files, which also have null sized references because they are not only not found in the Sound Archive's files, but not present in any group either, since the file is actually outside the Sound Archive altogether. This is most present in Super Mario Odyssey, where several Groups are stored outside the Sound Archive. Because of this system, it is important to make a system for your editor that can read these files within these Groups to fill in the gaps. I recommend a reference system, where you have a file class that can reference a parent file, all the way back to the main Sound Archive. This will make reading and writing these files easier. In a Group file though, a file can only be one of two type: Linked or Embedded. If a file has a null sized reference, then it is a linked file to load the file, wherever it may be outside of the group file. If it is an embedded file, then the file can load the file directly from the Group itself, since the file block contains it. Note that this can lead to a "double reference", where a file can be both Internal and inside a Group at the same time. This means that the Internal file offsets to the file contained inside a Group, and the Group offsets to this same file! It's important to keep this in mind when writing proper Sound Archives. A Group also contains a list of dependencies, for the files it is loading the dependencies for. Each dependency is an entry from the main Sound Archive.

## The Main File
The main file contains of a File Header, Info Block, a File Block, and an Extra Info Block. Each block is padded to 0x20 bytes. The structure of the file is the same for every version, with the standard being 1.0.0.

| **Type** | **Description** |
|----------|-----------------|
|FileHeader|Standard File Header (Magic: FGRP or CGRP. Always contains 3 blocks)|
|Block|Info Block (Reference Type: 0x7800)|
|Block|File Block (Reference Type: 0x7801)|
|Block|Extra Info Block (Reference Type: 0x7802)|

## Info Block Body (Magic: INFO)
This is perhaps the simplest info block you will ever come across. Padded to 0x20 bytes.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Table`<Reference`>|References to each File Entry (Reference Type: 0x7900)|
|0x04 + 8 * Table Count|FileEntry[Table Count]|File Entries referenced earlier|
|----|---|Padding for alignment|

### File Entry
An entry to a file in the file block must also indicated a file id, unlike in Wave Archives. So a file entry structure must then exist. Each entry is always 0x10 bytes long.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|u32|File id of this file|
|0x04|SizedReference|References to each file in the File Block (Reference Type: 0x1F00)|

## File Block Body (Magic: FILE)
All the files referenced from the Info Block are in here. Each offset is relative to the beggining of this block body. If the reference is null, then it's a null file and no file exists for it. Each file and this block itself are padded to 0x20 bytes, but the padding is not included in the size of the size reference. Any file with the same file id in the main Sound Archive is the same file.

## Extra Info Block Body (Magic: INFX)
This contains the dependencies that the files belong to. It tells the game to load certain files associated with an entry.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Table`<Reference`>|References to each Dependency (Reference Type: 0x7901)|
|0x04 + 8 * Table Count|Dependency[Table Count]|Dependencies referenced earlier|
|----|---|Padding for alignment|

### Dependency
An entry to a file in the file block must also indicated a file id, unlike in Wave Archives. So a file entry structure must then exist. Each entry is always 0x8 bytes long.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Id|Id of the entry in the main Sound Archive|
|0x04|Load Flags|Flags of what to load from the entry above|

#### Load Flags
These tell what to load from an entry. It is a u32, where each bit tells what to load. If everything is to be loaded, then a value of 0xFFFFFFFF is used.

| **Bit Index** | **What To Load** |
|---------------|------------------|
|0|Sequence|
|1|Wave Sound Data|
|2|Bank|
|3|Wave Archive|

## Summary
A tree to sum up the structure of the file:
```
GRP File:
|-File Header
|
|-Info Block
|    |-Table of References to each File Entry.
|        |-File id and Side References to each file in the file block.
|
|-File Block
|    |-Collection of files referenced from the info block.
|
|-Extra Info Block
|    |-Table of References to each Dependency.
|        |-Dependency containing an Id and Load Flags
|
|-Files
```