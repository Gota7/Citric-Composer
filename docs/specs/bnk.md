# Banks (.bfbnk, .bcbnk)
BNKs, or banks, contain a list of instruments that can be used with a sequence in order to play level music, sound effects, etc. Each instrument has info about how referenced waves from wave archives are to be played, and where to use them in the note range of the instrument.

## The Main File
The main file contains of a File Header and an Info Block. The Info Block is not padded. The structure of the file is the same for every version, with the standard being 1.0.0.

| **Type** | **Description** |
|----------|-----------------|
|FileHeader|Standard File Header (Magic: FBNK or CBNK. Always contains 1 block)|
|Block|Info Block (Reference Type: 0x5800)|

## Info Block Body (Magic: INFO)
Contains references to a wave id table and a reference table for each instrument. The instrument reference table is written before the wave id table. If a reference is null, then it implies that the the corresponding list is null.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Reference|Reference to wave id table (Reference Type: 0x0100)|
|0x08|Reference|Reference to the instrument reference table (Reference Type: 0x0101)|

### Instrument Reference Table
As the name implies, it is a table of references to each instrument. Notice, that if an instrument even has a null reference, it has the reference type 0x5903.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Table`<Reference`>|Table of References to each Instrument (Reference Type: 0x5900)|

#### Instrument
Contains a reference to the detailed instrument type. It could be a direct, index, or range instrument depending on the reference type.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Reference|Reference to the detailed data. The reference type is 0x6000 for Direct, 0x6001 for Range, and 0x6002 for Index|

##### Direct Instrument
Contains a reference to the key region.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Reference|Reference to the key region (Reference Type: 0x5901)|

##### Range Instrument
Contains a range of key regions for an instrument.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Table`<u8`>|Indices of the ranges|
|----|Reference[Table Count]|References to the key regions (Reference Type: 0x5901)|

##### Index Instrument
Contains an index of key regions for an instrument.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|s8|Start note|
|0x01|s8|End note|
|0x02|u16|Padding|
|0x04|Reference[End note - Start note + 1]|References to the key regions (Reference Type: 0x5901)|

###### Key Region
Contains a reference to the detailed key region type. It could be a direct, index, or range key region depending on the reference type.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Reference|Reference to the detailed data. The reference type is 0x6000 for Direct, 0x6001 for Range, and 0x6002 for Index|

####### Direct Key Region
Contains a reference to the velocity region.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Reference|Reference to the velocity region (Reference Type: 0x5902)|

####### Range Key Region
Contains a range of velocity regions for a key region.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Table`<u8`>|Indices of the ranges|
|----|Reference[Table Count]|References to the velocity regions (Reference Type: 0x5902)|

####### Index Key Region
Contains an index of velocity regions for a key region.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|s8|Start velocity|
|0x01|s8|End velocity|
|0x02|u16|Padding|
|0x04|Reference[End note - Start note + 1]|References to the velocity regions (Reference Type: 0x5902)|

######## Velocity Region
This contains how to play a part of the given key region.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|u32|Index of the wave from the wave id table to use|
|0x04|BitFlag|Flags for the velocity region|

######### Velocity Region Bit Flags
Conains information on how to play the velocity region. The flags are hardcoded to be constant in the NintendoWare Library.

| **Flag Bit** | **Used Frequency**| **Value Range** | **Description** |
|--------------|-------------------|-----------------|-----------------|
|0|Always|0-127|Original key of the wave|
|1|Always|0-127|Volume to play the wave at|
|2|Always|0-0xFFFF|Pan information. It's 0x0000SSPP where S is surround pan, and P is pan|
|3|Always|0-0xFFFFFFFF|Pitch. It's actually parsed as an f32|
|4|Always|0-0x00010F01|Velocity information. It's 0x00IIKKPP where I is the interpolation type, K is the key group, and P is if the velocity region is in percussion mode|
|8|Never|-----|Send Value|
|9|Always|0-0xFFFFFFFF|Offset to ASHDR curve reference from the beginning of the Velocity Region, should always be 0x20|
|10|Never|0-0xFFFFFFFF|Randomizer|
|11|Never|0-0xFFFFFFFF|LPF Info|

########## Interpolation Type (Enumeration)

| **Value** | **Description**|
|-----------|----------------|
|0|Polyphase|
|1|Linear|

########## ADSHR Curve Reference
Literally just a reference to the ADSHR curve data.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Reference|Reference to ADSHR curve data (Reference Type: 0)|

### Wave Id Table
As the name implies, it's just a simple table of wave ids. These are to be used by instruments.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|Table`<WaveId`>|Table of wave ids to use with the instruments|

## Summary
A tree to sum up the structure of the file:
```
BNK File:
|-File Header
|
|-Data Block
|    |-Table of instrument references.
|    |   |-Instrument with reference to detailed data.
|    |       |-Direct, Range, or Index Info
|    |           |-Key region with reference to detailed data.
|    |               |-Direct, Range, or Index Info      
|    |
|    |-Wave id table.
```