# quicksheet-guid

A [QuickSheet](https://github.com/cemheren/QuickSheet) extension that generates GUIDs/UUIDs on demand.

## Usage

Type in any cell:

| Cell content | Output |
|---|---|
| `guid:` | Single GUID (e.g. `a1b2c3d4-e5f6-7890-abcd-ef1234567890`) |
| `guid: 5` | Generate 5 GUIDs in consecutive rows |
| `guid: n` | No-dash format (`a1b2c3d4e5f67890abcdef1234567890`) |
| `guid: n 3` | 3 GUIDs in no-dash format |
| `guid: b` | Braced format (`{a1b2c3d4-e5f6-...}`) |
| `guid: upper` | Uppercase GUID |
| `guid: upper 10` | 10 uppercase GUIDs |

Maximum 20 GUIDs per invocation.

## Install

Clone this repo into your QuickSheet extensions directory:

```bash
git clone https://github.com/Deskworks/quicksheet-guid ~/.quicksheet/extensions/quicksheet-guid
```

## Requirements

- .NET 9 SDK
- [QuickSheet](https://github.com/cemheren/QuickSheet)

## License

MIT
