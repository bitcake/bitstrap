# Changelog

### 1.18.0

+ InlineScriptableObjectAttribute: show ScriptableObject editors inline as property


### 1.17.1
* AnimationParameterDrawer: fix warning when GameObject is disabled or when there's no AnimatorController


### 1.17.0

* TweenShader: now it's possible to edit textures tiling and offset
* Option: fix not working correctly with Unity Objects after they're destroyed
* Timer: rework and api simplification
* Timer.OnUpdate(): renamed to Timer.Update(), also it enables you to check if the timer completed inside an if
* Timer.IsRunning: renamed to Timer.isRunning


### 1.16.0

* Changed folder structure to better accommodate AssemblyDefinition files


### 1.15.1

+ FieldWidth: a scoped change to field width that will behave like EditorGUIUtility.fieldWidth
* Fix TweenShader not being able to change properties in inspector
* Fix TweenShader index out of range when no Renderer was selected
* UnitDrawer: always receives a Style parameter
* SteamUploadBuildAction: now possible to get steam credentials from cli through the args "steam-username" and "steam-password"
* EditorPrefProperty: moved into the "#Core" folder
* EditorHelper: moved into the "#Core" folder
* RectExtensions: moved into the "#Core" folder
* StaticReflectionHelper: moved into the "#Core" folder
* FileSystemHelper: moved into the "#Core" folder
* Moved EditorUtil folder into the "#Core" folder
* SteamUploadBuildAction: added skipSteamContentCopy to skip the steam content copy phase before upload. Useful when you build directly to the content folder. Also, it is now possible to override some values when invoking UMake from the cli.
* Proeprty: renamed from PropertyGUI


### 1.15.0

+ ResultExamples: Added some Result examples
+ UnityWebRequestHelper: helper class for compatibility between Unity versions
+ BoxGroup: editor code inside using(BoxGroup.Do()) will be drawn inside a nice editor box
+ IndentLevel: editor code inside using(ChangeIndentLevel.Do()) will be drawn with a different indentation
+ LabelWidth: editor code inside using(ChangeLabelWidth.Do()) will be drawn with a different label width
+ DisabledGroup: editor code inside using(DisabledGroup.Do()) will behave like inside a EditorGUI.BeginDisabledGroup()
+ FadeGroup: editor code inside using(FadeGroup.Do()) will behave like inside a EditorGUI.BeginFadeGroup()
+ Horizontal: editor code inside using(Horizontal.Do()) will behave like inside a EditorGUI.BeginHorizontal()
+ PropertyGUI: editor code inside using(PropertyGUI.Do()) will behave like inside a EditorGUI.BeginProperty()
+ ScrollView: editor code inside using(PropertyGUI.Do()) will behave like inside a EditorGUI.BeginScrollView()
+ Vertical: editor code inside using(Vertical.Do()) will behave like inside a EditorGUI.BeginVertical()
* EditorHelper: refactored some methods to a class inside Plugins/Inspector/Editor/EditorUtil
* WebAction.Request(): now returns a Promise instead of WebRequest
* RuntimeConsole: now has a keyCombination property
- WebRequest: a Promise is used in its place now


### 1.14.1

* Fix compatibility issues with both Unity 2017.1 and Unity 2017.3


### 1.14.0

+ Promise: a Future/Promise monad implementation


### 1.13.0

+ Result: a monad that contains either a value or an error. Behaves like the Result monad in Rust
* Option: now it behaves like the Option monad in Rust
* WebApi: now it uses the Result monad in its methods and callbacks
* UMakeCli.BuildAndPostBuild(): will build and immediately execute post build actions
- Callback: was removed because we do not encourage the use of null references


### 1.12.0

+ Timer.Start( timeOffset ): start the timer with a time offset (in the future)
+ Timer.onTimerPrecise: same as 'onTimer' but takes the callback latency as a parameter
* NumberRange: renamed from NumberBounds
* Fix TweenShader Editor: it was not saving changes to the animation curve


### 1.11.2

+ Documentation links "Windows/BitStrap/ Open Web Documentation" and "Preferences/BitStrap"
+ "Assets/BitStrap/Documentation/Documentation.html" that points to the web docs
* Changelog is a .txt again


### 1.11.1

+ Unity 5.6.2 support
+ Unity 2017.2 support
+ More UMakeBuildTarget examples


### 1.11.0

+ BlobSerializer: simple text serializer that handles circular references
+ WebApi URL params: if you pass more parameters than the ones defined in a WebAction, they're passed directly in the url
+ WebActionAttribute: now you write the HTTP method explicitly in the WebAction attribute
* Collections.Iter(): renamed from Collections.Each() (includes List<T>, Dictionary<K,V> and HashSet<T>)
* CircularBuffer.Enumerator: enumerator is now GC free


### 1.10.0

New extensible build system: UMake!
+ UMake: make builds by setting up presets and pre/post build actions
+ References: a handy way to get references to assets in a folder
+ NonNullableDrawer: draws a warning in inspector if a field is left null
* Improvements to WebApi


### 1.9.3

Windows Store App (WSA) support!
+ TypeExtensions: methods for WSA compatibility
+ MemberInfoExtensions: methods for WSA compatibility
+ BitArrayExtensions: methods for WSA compatibility
* Type.GetCustomAttribute: renamed to GetAttribute
* MemberInfo.GetCustomAttribute: renamed to GetAttribute


### 1.9.2

+ BitStream: serialize bools, ints and floats with bit precision. Good for critical network data compressing.
* Some editor scripts were outside the Editor folder causing build compilation issues.


### 1.9.1

+ Option: option (maybe) monad that represents the possibility of no value (better than passing null around)
+ Sort Components: right from the component context menu, you can sort your components again!
+ You can now open a scriptable object inspector by double clicking it
+ FolderPathAttribute: easily set a folder path in a script
* Fixed the script creator template error on Unity 5.6
* Some general code cleanup


### 1.9

+ RequireInterfaceAttribute: use it on a UnityEngine.Object reference to restricting its assignment
+ ShowImplementedInterfacesAttribute: show in inspector all the interfaces a component implements
+ TweenFov: tween the field of view of Camera components
+ "Tween" folder and tween examples
+ RectExtensions.Center: given a Rect, it returns a center anchored copy with a width of "width"
* TweenShader: super overhaul. Now you can even test the tween from the editor (no play required).
* TweenPath: adding methods to play through the end and also backward


### 1.8

+ HashSetExtensions: similar to DictionaryExtensions but for HashSet
+ TransformEditor: similar to the stock editor but with individual reset buttons next to each property
+ Collision/TriggerListener: enables you to control exactly what scripts receive Collision/Trigger events and from which colliders
+ ComponentReference: saves a reference to a component that can be easily retrieved where ComponentReference is located
* Timer: now works normally even if you call "OnUpdate()" more than once on Update
* AnimatorProperties: they now can be used even if there's no sibling Animator component
- ParticleSystemHelper: its functionality was not compliant with Unity's new way of handling ParticleSystems
- "Sort Components", "Move to Top", "Move to Bottom": this feature was a "hack". Since 5.5, it's broken because of some Unity internal refactorings.


### 1.7

+ TweenPath: Interpolates a transform through a Bezier or linear path of control points
+ Fix small bug on Modifiable
+ Some other fixes and code improvements
> Shout out to Johannes


### 1.6

+ Fix WebPlayer not compiling error.


### 1.5

+ BitStrap.Examples namespace: all examples are now inside of this namespace.
+ CircularBuffer: An insert optimized queue.
* Lots of small improvements


### 1.4

+ PropertyDrawerHelper: Methods that help to code a PropertyDrawer editor.
+ ParticleSystemExtensions: Extensions to the UnityEngine.ParticleSystem class.
* Timer: Added Progress property. 0.0 when the timer just started to 1.0 when the timer finished and stopped.
* TimerDrawer: Enhanced the editor.
* RectExtensions: Left() and Right() behaviours were swapped when "width" was negative. This is more intuitive.


### 1.3

+ StaticReflectionHelper: Bunch of static reflection helper methods.
+ ScriptDefinesHelper: Helper to work with scripting define symbols.

+ PlayerPrefsProperties: Makes it easy to work with PlayerPrefs treating them as properties.
+ EditorPrefsProperties: Makes it easy to work with EditorPrefs treating them as properties.

+ ScriptCreator: Create C# Script and C# Editor Script through the "Assets > Create" menu.


### 1.2

+ ScriptableObjectCreator: create ScriptableObject instances by right-clicking its scripts;
+ RectExtensions: helper extensions for non-layout editor codes;
+ ReadOnlyAttribute: put this on a field of your script. That field will appear as read only;
+ HelpBoxAttribute: put this on a field of your script. It will draw a EditorGUI.HelpBox above it;
+ ModifiableInt and ModifiableFloat: a specialized and serializable version of Modifiable<T>;
+ SerializedPropertyHelper: a method that returns the current property value. Used in NumberBoundsDrawer and ModifiableDrawer;
+ BackgroundBlurEditor: custom editor for BackgroundBlur.shader

* ColorExtensions: is now ColorHelper;
* ListExtensions: Added methods Count(), Any(), All() and ToStringFull();
* DictionaryExtensions: Added methods Count(), Any(), All() and ToStringFull();
* StringHelper: Get() is now Format();
* Modifiable<T>: Changed to Math folder; and created a nice Inspector editor;


### 1.1

First public version.
