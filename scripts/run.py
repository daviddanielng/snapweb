import sys
from typing import Literal


AllowedArguments = Literal[
    "--help", "--cli-dev", "--gui-dev", "--cli-prod", "--gui-prod"
]


def main():
    start(get_argument())


def get_allowed_arguments() -> list[str]:
    return [
        "--help",
        "--cli-dev",
        "--gui-dev",
        "--cli-prod",
        "--gui-prod",
    ]


def print_to_color(
    text: str,
    color: Literal["red", "green", "yellow", "blue", "magenta", "cyan", "white"],
):
    colors = {
        "red": "\033[91m",
        "green": "\033[92m",
        "yellow": "\033[93m",
        "blue": "\033[94m",
        "magenta": "\033[95m",
        "cyan": "\033[96m",
        "white": "\033[97m",
    }
    color_code = colors.get(color, "")
    reset_code = "\033[0m"
    print(f"{color_code}{text}{reset_code}")


def run_command(command: str):
    import subprocess

    process = subprocess.Popen(
        command,
        shell=True,
        stdout=sys.stdout,
        stderr=sys.stderr,
    )
    process.wait()
    if process.returncode != 0:
        print_to_color(f"Command exited with code {process.returncode}", "red")


def start(argument: AllowedArguments):
    current_dir = sys.path[0]
    remaining_args = sys.argv[2::]
    if current_dir.endswith("scripts"):
        current_dir = current_dir[: -len("scripts")]
    print_to_color(f"Current directory: {current_dir}", "cyan")
    command = ""
    match argument:
        case "--cli-dev":
            print_to_color("Running CLI in development mode...", "cyan")
            command = f"cd {current_dir}/cli/cli  && dotnet run {' '.join(remaining_args)}"
        case "--gui-dev":
            print_to_color("Running GUI in development mode...", "cyan")
            command = f"cd {current_dir}/gui/  && cargo run {' '.join(remaining_args)}"
        case "--cli-prod":
            print_to_color("Running CLI in production mode...", "cyan")
            command = f"cd {current_dir}/cli/cli && dotnet run --configuration Release {' '.join(remaining_args)}"
        case "--gui-prod":
            print_to_color("Running GUI in production mode...", "cyan")
            command = f"cd {current_dir}/gui/ && cargo run --release {' '.join(remaining_args)}"
        case _:
            print_to_color(f"Invalid argument: {argument}", "red")
            help()
            sys.exit(1)
    run_command(command)


def help():

    print("""
Usage: python run.py [OPTIONS] [-- ARGS...]

Options:
  --help            Show this help message and exit
  --cli-dev         Run the CLI in development mode
  --gui-dev         Run the GUI in development mode
  --cli-prod        Run the CLI in production mode
  --gui-prod        Run the GUI in production mode

Additional arguments after the option will be passed to the program.

Examples:
  python run.py --cli-dev 
  python run.py --cli-dev --verbose --output=file.txt
  python run.py --gui-prod -- --fullscreen --debug
""")


def get_argument() -> AllowedArguments:
    args = sys.argv[1::]
    command = args[0] if len(args) > 0 else None
    if command is not None:
        if command == "--help":
            help()
            sys.exit(0)
        elif command == "--cli-dev":
            return "--cli-dev"
        elif command == "--gui-dev":
            return "--gui-dev"
        elif command == "--cli-prod":
            return "--cli-prod"
        elif command == "--gui-prod":
            return "--gui-prod"
    print(f"Invalid argument: {command}")
    help()
    sys.exit(1)


if __name__ == "__main__":
    main()
else:
    print(
        "This script is being imported as a module, you have to run it directly to execute the main function."
    )
