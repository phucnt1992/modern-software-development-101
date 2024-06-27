#!/bin/bash

# ignoredups for history
if [ -f ~/.zshrc ]; then
echo "Setting up zsh history options"
cat <<EOF >> ~/.zshrc
  setopt HIST_EXPIRE_DUPS_FIRST
  setopt HIST_IGNORE_DUPS
  setopt HIST_IGNORE_ALL_DUPS
  setopt HIST_IGNORE_SPACE
  setopt HIST_FIND_NO_DUPS
  setopt HIST_SAVE_NO_DUPS
EOF
fi

# install npm packages
task init
