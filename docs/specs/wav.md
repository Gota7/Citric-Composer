# Wave (.bfwav, .bcwav) TODO
WAVs, or Waves, contain actual raw audio data, be it for music or sound effects. The data can be in standard PCM form, or encoded.

## The Main File
The main file contains of a File Header, an Info Block, and a Data Block. The Info Block is padded to 0x20. The standard version is 1.0.0, but from 1.2.0 on, there is an extra info parameter for the original loop start.