# Contributing to BitStrap

BitStrap is an evolving set of Unity tools. As such, it needs contributors and is intended to be easy to
contribute to. Help is welcome.

* [Testing](#testing)
* [Finding what to contribute](#finding-what-to-contribute)
* [Guidelines](#guidelines)

## Testing

To start, clone BitStrap from git and open that Unity project:
```
git clone git@github.com:bitcake/bitstrap.git
```

Open that project in Unity.

Alternatively, open any test scene you can find inside the folder
```
bitstrap/Assets/BitStrap/Examples/
```

## Finding what to contribute

There are three simple ways one could contribute to BitStrap:

### Keep BitStrap updated with new versions of Unity

Whenever a new version of Unity is available, an old api might break or be deprecated.
It's important to always keep BitStrap in sync with these updates so that newcomers never
get an error message in their face when importing BitStrap into their project. :)

### Adding your own useful tools

BitStrap is not limited to the tools developed at BitCake. We invite you to contribute any
script/tool you find useful in your project to BitStrap. This way, other developers can share
all the awesome workflows you made! :)

### Adding/Improving an example

Examples are a great way to learn how a code/tool works. Add your own example of how you use
BitStrap tools and make it easier for other to pick it up too! :)

All examples are found inside the folder
```
bitstrap/Assets/BitStrap/Examples/
```

### Other ways to contribute

And of course if you find bugs or ways to improve existing code, please do so! The more eyes
we can get the better quality we get.

## Guidelines

Consistency is king. Also, it helps other to reason about this project. There's not much to
talk about guidelines but here are a few things:

- All codes live inside the `BitStrap` namespace
- All example codes live inside the `BitStrap.Examples` namespace
- Keep the `Examples` folder structure as a mirror of the `Plugins` folder
  - That is, if you add a new tool (and a folder) named `foobar` to `Plugins`, please also
    add a `foobar` folder to `Examples` with examples of how to use it. There you're allowed
    to put scripts, scenes, prefabs, etc.
- If you're creating a "standalone tool" (like `UMake` for instance) you can put it in its own
  folder right inside the `Plugins` folder. On the other hand, if it's just a simple snippet, please
  put it toguether with other similar scripts.  
  - That is, if it's an extension class, put it inside `Plugins/Extensions`, if it's a math related
    snippet, put it inside `Plugins/Math`, and so on

## Thank you for contributing!

Thank you for taking your time and investing on this project. It makes us really happy and
glad to be part of the awesome community that is indie gamedev. Keep rockin!

** This `CONTRIBUTING.md` was based on [rust-cookbook's](https://github.com/rust-lang-nursery/rust-cookbook/blob/master/CONTRIBUTING.md)
