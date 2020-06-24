# Byter

- Inspect file byte 

## Objective

- Print byte information from file, and check the BOM

## Environment & Dependency 

- **.NETCore3.1**

## Features

- Check whether the file has BOM or not.
- Check which BOM the file has.
- Print length of file bytes. 
- Print file bytes using binary notation.
- Print file bytes using decimal notation.
- Print file bytes using hexadecimal notation.
- Print file bytes using integer notation.
- Print file bytes using double(IEEE double-precision) notation.

## Usage


### Ubuntu install

```bash
git clone https://github.com/KIMGEONUNG/byter
cd byter
dotnet build
```

- Type at cmd or bash
```
byter --help
```

**integer representation example**
```bash
> byter -i AL_11_D002_20200502.shp --range 0-99 --int --endian big
9994 0 0 0 0 0 103368530 -402456576 83886080 1622679173 2145191233 1622821132 1218452033 13990121 1700989505 542937026 -109503423 0 0 0 0 0 0 0 0
```
