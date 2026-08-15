#!/bin/bash

root="$1"

src="$root"".githooks/pre-commit"
dest="$root"".git/hooks/pre-commit"

if [[ ! -f "$dest" ]]
then
  cp "$src" "$dest"
  chmod +x "$dest"
fi
