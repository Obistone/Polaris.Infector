# Polaris Infector
Injects a downloader in the entrypoint of any .NET executable.

# How to use

Simply click on on the "File To Infect" input field and select your preferred .NET executable.

[![Title](https://i.imgur.com/wXHoK3A.png)]()

Once done, you'll need to have a direct link to your executable file of choice. This file will be downloaded, dropped, and executed.

# Credits

Thanks to [NotSoonTM](https://github.com/NotSoonTM/ "NotSoonTM") for helping me a ton with this project.

This project implements [dnlib](https://github.com/0xd4d/dnlib "dnlib") for the injection, [MaterialDesignInXaml](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit "MaterialDesignInXaml") for UI, and [Costura.Fody](https://github.com/Fody/Costura "Costura.Fody") for assembly linking.


# Disclaimer

This is simply a PoC, it is not meant for practical use. 

Your payload would have to be runtime FUD for anyone to execute it without AVs blaring.
