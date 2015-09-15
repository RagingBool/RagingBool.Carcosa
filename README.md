# RagingBool.Carcosa 0.2.0.0 [NOT RELEASED]
Carcosa is a lighting driving software that is built as part of the Unbirthday Camp.

***Note***: *This application is in it's 0.X version, that means that it's still in active development and backward compatibility is not guaranteed!*

## Links
* Git repository: https://github.com/RagingBool/RagingBool.Carcosa
* All Raging Bool Git repositories: https://github.com/RagingBool

## Main features

## License and Copyright
Apache License 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
Copyright 2015 Raging Bool (http://ragingbool.org)

## Release Notes
### Version 0.1 

* **Version 0.2.0.0** [NOT RELEASED]
  * Control
    * Refactoring the control board to use Epicycle.Input
    * Creating a computer keyboard based controller
    * Support for overlapping multiple controllers
  * Devices infrastructure refactoring and rewriting resulting in a much more cleaner light control protocols implementation
  * Addinf Akka and initialing an AcrorSystem
  * Utilizing Epicycle.Input
  * Upgrading Epicycle libs

* **Version 0.1.1.0** [2015-07-01]
  * Initial support for Fadecandy server
  * Lots of little things for Midburn 2015, some of them very specific

* **Version 0.1.0.0** [2015-04-04]
  * Creating a basic WPF app
  * Support for AKAI LPD8 MIDI controller
  * Support for Snark over serial port
  * Support for DMX over E1.31
  * Very simple hard-coded party mode
  * Magical Forest POC
  * LED Matrix POC
  * KNOW ISSUES
    * Concurrency problems
    * Interacting with the MIDI controller sometimes frezzes the software