default:
    just --list
local-list:
    echo "local p2p types:"
    echo "arena"
    echo "story"
    echo "meadow"
local type:
    dotnet build -c {{ if type == "arena" { "ArenaP2P" } else if type == "story" { "STORYP2P" } else { "LOCALP2P" } }}
build:
    dotnet build
clean:
    dotnet clean
