I use some tools for Windows, maybe it can be helpful for you.

1) sudo - allow you to run programs with admin rights. For example you open console, run some commands and need to run one command with admin rights. From my perspective Disable UAC is not a good idea.

2) RunAsx86 - runs dotnet AnyCpu applications in x86 mode, sometimes you need it, for tests or to work with other programs, which require this.

Also those tools provide command line arguments.

To use this tools, it is a good idea to move them into separate folder and add path to this folder to Path environment variable.
