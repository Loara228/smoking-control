
# Guide for Windows

```<Domain.com>``` - Your domain

# 🛠️ Steps to run

1. Download and install:
   - Visual Studio
      - .NET SDK
      - .NET MAUI
2. Clone the repository
3. Set up the launch configuration

    Edit configuration

    ```bash
    # git bash
    cd smoking-control/build
    nano conf.sh
    ```

    Set your IP

    ```txt
    export SRV_HOSTNAME="192.168.0.?"
    ```

    or domain
    
    ```txt
    export SRV_HOSTNAME="<Domain.com>"
    ```

    ```bash
    ./conf_maui.sh
    ```

    If you use http (android manifest)

    ```xml
    android:usesCleartextTraffic="true"
    ```

4. Build for Windows or Android
    ```cmd
    dotnet publish -f net9.0-windows10.0.19041.0 -c Debug -p:PublishReadyToRun=true -p:WindowsPackageType=None
    ```

    ```cmd
    dotnet publish -f net9.0-android -c Release
    ```