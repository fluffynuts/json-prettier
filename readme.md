json-prettier
---

An offline, cli json prettier

build
---
`npm run publish`
or
`npm run build` (if you want something dotnet-dependent)

_ignore the warnings about trimming and JsonSerializer, unless you get issues afterwards - Works Great On My Machine (tm)_

usage
---

cat /path/to/file.json | json-prettier
or
json-prettier /path/to/file.json

will output on stdout
