
# 🚬 Smoking control

This project aims to reduce nicotine dependence.

## ⭐ Why this project architecture?

I rarely use my phone and can forget about it for 6-8 hours. However, outdoors, you can't just pull out a laptop from your pocket to use the app.<br>
Therefore the application needs to be available on mobile device (Android) and PC (Windows). For this purpose, [.NET MAUI](https://dotnet.microsoft.com/en-us/apps/maui) is employed to develop a cross-platform application for all necessary devices. Data is stored in [PostgreSQL](https://www.postgresql.org/), with [Actix Web](https://actix.rs/) handling communication between the client and server

## 💡 What does the application do?

The app displays the elapsed time since the last nicotine intake. It also shows a recommended interval that increases over time. Before attempting to break this interval and smoke again, the app will provide a warning to help prevent this. Additionally, the app allows users to view statistics and logs

**Important**: Users should take this seriously and manually open the app and log each smoking event, otherwise the application will not help you much

## Web

[guide](./guide.md)

## Windows

1. Download and install:
   - Visual Studio
      - .NET SDK
      - .NET MAUI
2. Clone the repository
3. Set up the launch configuration
    ```bash
    # git bash
    cd smoking-control/build
    nano conf.sh
    ./conf_maui.sh
    ```
4. Build for Windows or Android
    ```cmd
    dotnet publish -f net9.0-windows10.0.19041.0 -c Debug -p:PublishReadyToRun=true -p:WindowsPackageType=None
    ```

    ```cmd
    dotnet publish -f net9.0-android -c Release
    ```

<hr>

```
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠿⠟⠛⠋⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠙⠛⠿⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠿⠋⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠉⠛⠿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠟⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠛⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠛⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠹⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣀⣀⠀⣀⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⠻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣤⣴⡄⠸⣿⣿⣇⠸⣿⣿⡏⢰⣿⠇⣸⣿⡿⢀⣿⡆⢰⡆⢠⣄⠀⠀⠀⠙⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⠐⢾⣆⠘⠿⠛⠀⠙⡋⠉⠀⠉⠉⠀⣚⣛⣀⣙⣛⡃⠘⠿⠀⠿⠃⣼⡏⢰⣷⠀⡀⢹⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⣶⡈⢻⣧⠈⠉⣀⣂⣭⣥⣶⣶⣿⣿⣿⣿⣶⣦⣭⣭⣭⣭⣭⣭⣭⡃⣶⣦⣬⡀⠻⠏⢀⡇⠈⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⢤⣶⣄⠹⠿⠀⣠⣶⣿⣿⣿⣿⠿⠿⠿⠿⠿⠿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠿⠿⠿⢿⣿⣿⣷⣄⡈⢁⠀⢸⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠀⠀⠀⠀⠀⠀⢀⠐⢷⣄⡉⠛⣁⣴⣾⣿⣿⣋⣥⣶⣶⣾⣿⣿⣿⣿⡖⢿⣿⣿⣿⣿⣿⣿⣯⢴⣿⣿⣿⣶⣌⠻⣿⣿⣧⠈⠀⢸⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠀⠀⠀⠀⠀⣄⠙⢷⡄⠋⣠⣾⣿⣿⣿⣿⣿⣿⠛⣛⣛⣋⣩⣿⣿⣿⣿⣆⢹⣿⣿⣿⣿⣿⣿⠀⣿⣿⣿⠿⠿⣿⣿⣿⣿⠀⠁⣾⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠀⠀⠀⢴⣦⡙⠳⠄⢀⣾⣿⣿⣿⣿⣿⣿⢿⣿⣿⣿⠿⠿⠿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣤⣿⣿⣿⣿⣷⣶⣬⣙⣻⠇⣸⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠀⠀⣶⣄⠙⢿⡆⢠⣿⣿⠿⣿⣿⣿⣿⣃⣀⣨⣇⠐⢶⠀⠀⠀⠀⣠⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⠋⣉⡉⠛⠛⠛⣿⡿⢯⠀⣿⣿⣿⣿⣿⣿⣿⢿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣇⠀⣦⠘⢿⣶⠄⢠⣾⣿⣿⡎⠘⠛⣿⣿⣿⣿⣿⡏⣁⣀⢀⣀⣀⠀⠀⠈⣿⣿⣿⣿⣿⣿⣿⣿⣏⠠⡉⠁⠀⠀⠀⣿⣄⠛ ⢿⣿⣿⣿⣿⣿⡏⣼⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣆⠹⣷⣄⠙⢠⣿⣿⣿⣿⡧⠈⡀⣻⣿⣿⣿⣿⣧⣬⣍⣀⠀⠀⠈⢀⣼⣿⣿⣿⣿⣿⣿⣿⠈⣿⣦⣀⠈⠁⠀⠰⠛⣿⣿⡇⠈⣿⣿⣿⣿⣿⠇⣾⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣆⠹⣿⡇⢸⣿⣿⣿⣿⡏⠀⠓⢌⠼⢻⣿⣿⣿⣿⣿⣿⣷⣶⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⡄⠹⣿⣿⣦⣼⣿⣿⣿⣿⣿⣥⠀⣿⣿⣿⣿⠟⠁⣰⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣦⡈⠃⣸⣿⣿⣿⣿⡇⢸⡓⣎⡇⠧⠙⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣄⠙⢿⣿⣿⣿⡿⢷⡿⣏⠢⠀⣿⣿⡏⠌⣴⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠦⠘⣿⣿⣿⣿⣇⠈⠼⡐⢛⢀⠺⣿⡙⢿⣿⣿⣿⣿⣿⣿⠟⢛⣉⣿⣿⣿⣿⣿⣿⣿⣷⠀⣿⣿⣿⣿⣮⡀⢠⠅⣰⣿⣿⠘⣼⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣇⡀⢸⣿⣿⣿⣿⡆⠨⢼⣆⡻⣦⠹⣛⣧⣿⣿⣿⣿⡟⢿⡄⠹⣿⣿⡿⠿⢿⣿⣿⣿⠁⣼⣿⣿⣏⡟⡥⠀⠀⢠⣿⣿⡆⢿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠁⢸⣿⣿⣿⣿⣷⡄⠸⣡⠯⠛⣖⡆⣽⣿⣍⣿⡟⢡⡾⢿⠶⣶⣾⣗⣿⡿⣛⣶⣤⣼⡁⣿⣟⣋⠌⡶⠄⢠⣿⣿⡟⠈⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠃⢸⣿⣿⣿⣿⣿⠹⡄⠁⠜⣷⣾⢻⣭⡀⣶⡟⣰⣿⡉⣟⢋⣤⣼⡮⠂⢖⡋⠓⠺⠿⣇⢹⣷⡃⡅⠃⣰⣿⣿⣿⡗⢀⢸⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣏⠀⢸⣿⣿⣿⣿⣿⡇⣷⢤⡄⠉⠻⢃⢥⡛⣾⣤⣿⣿⣁⣌⣙⡛⠻⠿⠿⠿⠿⠛⠐⠂⢌⡀⢿⡩⠄⣴⣿⣿⣿⣿⡿⢸⠘⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠿⠛⠛⠋⠁⠀⣾⣿⣿⣿⣿⣿⠀⣗⣿⣿⣧⣄⠀⠁⢳⣬⠘⢻⣿⣿⢿⣿⣿⣏⡙⣉⢙⣛⣁⡒⣌⡀⢍⠫⠉⠀⣿⣿⣿⣿⣿⠇⠜⢠⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⠿⠟⠉⠁⠀⠀⠀⠀⠀⠀⣼⣿⣿⣿⣿⣿⠧⢴⠿⡼⢞⣦⣙⣏⣦⠀⠻⢳⣖⣾⠻⣯⣝⢳⣩⡿⡿⢛⣻⢟⠿⡏⣛⡆⠈⠂⢔⠻⣿⠟⡫⠔⣋⣴⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⠏⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣿⣿⣿⣿⣿⣿⣇⠲⠭⢶⣵⠖⣙⣬⡿⣧⣆⠀⠘⢾⢉⣿⣿⣆⣿⢋⣽⣿⣴⡏⣭⣧⠛⠃⢴⣿⣶⣍⢀⣤⣶⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⠟⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣾⣿⣿⣿⣿⣿⠿⠛⢉⣶⠽⣭⣾⣿⡷⢽⡻⣿⣷⣇⢄⢈⡙⠛⠇⠾⠿⠷⠻⠿⠟⠋⠁⠀⠀⠀⠙⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠚⠿⣿⣿⣿⣿⠁⣴⣿⣿⣿⣿⣿⣦⣿⣎⣢⡝⣶⣦⣹⣮⣠⠸⣟⣶⣶⣷⡦⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠻⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠁⢾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣿⣷⣝⢻⣿⣧⣘⠻⣿⣿⣿⣦⣀⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠙⠿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⣿⣾⣿⣷⣿⣾⡿⠟⠋⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠉⠉⠙⠛⠿⣿⣿⣿⣿⣿⣿⣿
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢉⣙⡛⠋⠩⠍⠻⠿⢿⡿⠟⣛⡛⠿⠛⠋⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉

                                    ┌───────────────────────────────────────────────────┐
                                    │Now Playing: Пачка Сигарет                         │
                                    │Виктор Цой                                         │
                                    │⏮  ▶ 1:49 / 4:43 ──⚬────                  ⚬ ⚬ ⚬ ⏭ │
                                    └───────────────────────────────────────────────────┘ 
```