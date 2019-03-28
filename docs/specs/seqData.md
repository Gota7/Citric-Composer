# TODO


### Note Command
The identifier is the note number.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|s8|Velocity, always positive|
|0x01|VariableLength|Length of the note in ticks, 48 is a quarter note|

### Wait Command
Waits for a particular amount of time.

| **Offset** | **Type** | **Description** |
|------------|----------|-----------------|
|0x00|VariableLength|Length to wait in ticks, 48 is a quarter note|

### End Command (Fin)
No parameters.