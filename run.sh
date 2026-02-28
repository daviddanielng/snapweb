#!/bin/bash

choice="$1"
shift 2>/dev/null
remaining_args="$@"

if [ -z "$choice" ]; then
    echo "What do you want to start?"
    echo "0) Help"
    echo "1) CLI in development mode"
    echo "2) GUI in development mode"
    echo "3) CLI in production mode"
    echo "4) GUI in production mode"    
    read -p "Enter the number corresponding to your choice: " choice
fi
if [ "$choice" -eq 0 ]; then
    python3 scripts/run.py --help
elif [ "$choice" -eq 1 ]; then
    python3 scripts/run.py --cli-dev $remaining_args
elif [ "$choice" -eq 2 ]; then
    python3 scripts/run.py --gui-dev $remaining_args
elif [ "$choice" -eq 3 ]; then
    python3 scripts/run.py --cli-prod $remaining_args
elif [ "$choice" -eq 4 ]; then
    python3 scripts/run.py --gui-prod $remaining_args
else
    echo "Invalid choice. Please run the script again and select a valid option."
fi
