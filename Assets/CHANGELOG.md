# Changelog

### 1.23
- fixed: serialization issue when having an AnimationParameter inside a struct that was automatically initialized by Unity.
- fixed: support for Unity 2020 LTS and C# versions < 9.0
- removed: NestedPrefab script
- removed: NullableAttribute and NonNullableDrawer
- added: RequiredReferenceAttribute and RequiredReferenceDrawer. Allows you to specify an Object field as required, will show a red warning if the field is not properly setup in the inspector

### 1.22
- added: ColliderImporter: Follows Unreal Engine's Collision Meshes setup rules to automagically add Colliders to models. (https://docs.unrealengine.com/4.27/en-US/WorkingWithContent/Importing/FBX/StaticMeshes/)

### 1.21.1
- fixed: shader editor prevented build

### 1.21.0
Now one needs to use the branch `upm` when importing in Unity's Package Manager
- fixed: repository + UPM package export

### 1.19.0
Migrating to Unity Package Manager

### 1.18.0
- added: InlineScriptableObjectAttribute: show ScriptableObject editors inline as property

### 1.17.1
- changed: AnimationParameterDrawer: fix warning when GameObject is disabled or when there's no AnimatorController

### 1.17.0
- changed: TweenShader: now it's possible to edit textures tiling and offset
- changed: Option: fix not working correctly with Unity Objects after they're destroyed
- changed: Timer: rework and api simplification
- changed: Timer.OnUpdate(): renamed to Timer.Update(), also it enables you to check if the timer completed inside an if
- changed: Timer.IsRunning: renamed to Timer.isRunning

### 1.16.0
- changed: Changed folder structure to better accommodate AssemblyDefinition files

### 1.15.1
- added: FieldWidth: a scoped change to field width that will behave like EditorGUIUtility.fieldWidth
- changed: Fix TweenShader not being able to change properties in inspector
- changed: Fix TweenShader index out of range when no Renderer was selected
- changed: UnitDrawer: always receives a Style parameter
- changed: SteamUploadBuildAction: now possible to get steam credentials from cli through the args "steam-username" and "steam-password"
- changed: EditorPrefProperty: moved into the "#Core" folder
- changed: EditorHelper: moved into the "#Core" folder
- changed: RectExtensions: moved into the "#Core" folder
- changed: StaticReflectionHelper: moved into the "#Core" folder
- changed: FileSystemHelper: moved into the "#Core" folder
- changed: Moved EditorUtil folder into the "#Core" folder
- changed: SteamUploadBuildAction: added skipSteamContentCopy to skip the steam content copy phase before upload. Useful when you build directly to the content folder. Also, it is now possible to override some values when invoking UMake from the cli.
- changed: Proeprty: renamed from PropertyGUI

### 1.15.0
- added: ResultExamples: Added some Result examples
- added: UnityWebRequestHelper: helper class for compatibility between Unity versions
- added: BoxGroup: editor code inside using(BoxGroup.Do()) will be drawn inside a nice editor box
- added: IndentLevel: editor code inside using(ChangeIndentLevel.Do()) will be drawn with a different indentation
- added: LabelWidth: editor code inside using(ChangeLabelWidth.Do()) will be drawn with a different label width
- added: DisabledGroup: editor code inside using(DisabledGroup.Do()) will behave like inside a EditorGUI.BeginDisabledGroup()
- added: FadeGroup: editor code inside using(FadeGroup.Do()) will behave like inside a EditorGUI.BeginFadeGroup()
- added: Horizontal: editor code inside using(Horizontal.Do()) will behave like inside a EditorGUI.BeginHorizontal()
- added: PropertyGUI: editor code inside using(PropertyGUI.Do()) will behave like inside a EditorGUI.BeginProperty()
- added: ScrollView: editor code inside using(PropertyGUI.Do()) will behave like inside a EditorGUI.BeginScrollView()
- added: Vertical: editor code inside using(Vertical.Do()) will behave like inside a EditorGUI.BeginVertical()
- changed: EditorHelper: refactored some methods to a class inside Plugins/Inspector/Editor/EditorUtil
- changed: WebAction.Request(): now returns a Promise instead of WebRequest
- changed: RuntimeConsole: now has a keyCombination property
- removed: WebRequest: a Promise is used in its place now

### 1.14.1
- changed: Fix compatibility issues with both Unity 2017.1 and Unity 2017.3

### 1.14.0
- added: Promise: a Future/Promise monad implementation

### 1.13.0
- added: Result: a monad that contains either a value or an error. Behaves like the Result monad in Rust
- changed: Option: now it behaves like the Option monad in Rust
- changed: WebApi: now it uses the Result monad in its methods and callbacks
- changed: UMakeCli.BuildAndPostBuild(): will build and immediately execute post build actions
- removed: Callback: was removed because we do not encourage the use of null references

### 1.12.0
- added: Timer.Start( timeOffset ): start the timer with a time offset (in the future)
- added: Timer.onTimerPrecise: same as 'onTimer' but takes the callback latency as a parameter
- changed: NumberRange: renamed from NumberBounds
- changed: Fix TweenShader Editor: it was not saving changes to the animation curve

### 1.11.2
- added: Documentation links "Windows/BitStrap/ Open Web Documentation" and "Preferences/BitStrap"
- added: "Assets/BitStrap/Documentation/Documentation.html" that points to the web docs
- changed: Changelog is a .txt again

### 1.11.1
- added: Unity 5.6.2 support
- added: Unity 2017.2 support
- added: More UMakeBuildTarget examples

### 1.11.0
- added: BlobSerializer: simple text serializer that handles circular references
- added: WebApi URL params: if you pass more parameters than the ones defined in a WebAction, they're passed directly in the url
- added: WebActionAttribute: now you write the HTTP method explicitly in the WebAction attribute
- changed: Collections.Iter(): renamed from Collections.Each() (includes List<T>, Dictionary<K,V> and HashSet<T>)
- changed: CircularBuffer.Enumerator: enumerator is now GC free

### 1.10.0
New extensible build system: UMake!
- added: UMake: make builds by setting up presets and pre/post build actions
- added: References: a handy way to get references to assets in a folder
- added: NonNullableDrawer: draws a warning in inspector if a field is left null
- changed: Improvements to WebApi

### 1.9.3
Windows Store App (WSA) support!
- added: TypeExtensions: methods for WSA compatibility
- added: MemberInfoExtensions: methods for WSA compatibility
- added: BitArrayExtensions: methods for WSA compatibility
- changed: Type.GetCustomAttribute: renamed to GetAttribute
- changed: MemberInfo.GetCustomAttribute: renamed to GetAttribute

### 1.9.2
- added: BitStream: serialize bools, ints and floats with bit precision. Good for critical network data compressing.
- changed: Some editor scripts were outside the Editor folder causing build compilation issues.

### 1.9.1
- added: Option: option (maybe) monad that represents the possibility of no value (better than passing null around)
- added: Sort Components: right from the component context menu, you can sort your components again!
- added: You can now open a scriptable object inspector by double clicking it
- added: FolderPathAttribute: easily set a folder path in a script
- changed: Fixed the script creator template error on Unity 5.6
- changed: Some general code cleanup

### 1.9
- added: RequireInterfaceAttribute: use it on a UnityEngine.Object reference to restricting its assignment
- added: ShowImplementedInterfacesAttribute: show in inspector all the interfaces a component implements
- added: TweenFov: tween the field of view of Camera components
- added: "Tween" folder and tween examples
- added: RectExtensions.Center: given a Rect, it returns a center anchored copy with a width of "width"
- changed: TweenShader: super overhaul. Now you can even test the tween from the editor (no play required).
- changed: TweenPath: adding methods to play through the end and also backward

### 1.8
- added: HashSetExtensions: similar to DictionaryExtensions but for HashSet
- added: TransformEditor: similar to the stock editor but with individual reset buttons next to each property
- added: Collision/TriggerListener: enables you to control exactly what scripts receive Collision/Trigger events and from which colliders
- added: ComponentReference: saves a reference to a component that can be easily retrieved where ComponentReference is located
- changed: Timer: now works normally even if you call "OnUpdate()" more than once on Update
- changed: AnimatorProperties: they now can be used even if there's no sibling Animator component
- removed: ParticleSystemHelper: its functionality was not compliant with Unity's new way of handling ParticleSystems
- removed: "Sort Components", "Move to Top", "Move to Bottom": this feature was a "hack". Since 5.5, it's broken because of some Unity internal refactorings.

### 1.7
Shout out to Johannes
- added: TweenPath: Interpolates a transform through a Bezier or linear path of control points
- added: Fix small bug on Modifiable
- added: Some other fixes and code improvements

### 1.6
- added: Fix WebPlayer not compiling error.

### 1.5
- added: BitStrap.Examples namespace: all examples are now inside of this namespace.
- added: CircularBuffer: An insert optimized queue.
- changed: Lots of small improvements

### 1.4
- added: PropertyDrawerHelper: Methods that help to code a PropertyDrawer editor.
- added: ParticleSystemExtensions: Extensions to the UnityEngine.ParticleSystem class.
- changed: Timer: Added Progress property. 0.0 when the timer just started to 1.0 when the timer finished and stopped.
- changed: TimerDrawer: Enhanced the editor.
- changed: RectExtensions: Left() and Right() behaviours were swapped when "width" was negative. This is more intuitive.

### 1.3
- added: StaticReflectionHelper: Bunch of static reflection helper methods.
- added: ScriptDefinesHelper: Helper to work with scripting define symbols.

- added: PlayerPrefsProperties: Makes it easy to work with PlayerPrefs treating them as properties.
- added: EditorPrefsProperties: Makes it easy to work with EditorPrefs treating them as properties.

- added: ScriptCreator: Create C# Script and C# Editor Script through the "Assets > Create" menu.

### 1.2
- added: ScriptableObjectCreator: create ScriptableObject instances by right-clicking its scripts;
- added: RectExtensions: helper extensions for non-layout editor codes;
- added: ReadOnlyAttribute: put this on a field of your script. That field will appear as read only;
- added: HelpBoxAttribute: put this on a field of your script. It will draw a EditorGUI.HelpBox above it;
- added: ModifiableInt and ModifiableFloat: a specialized and serializable version of Modifiable<T>;
- added: SerializedPropertyHelper: a method that returns the current property value. Used in NumberBoundsDrawer and ModifiableDrawer;
- added: BackgroundBlurEditor: custom editor for BackgroundBlur.shader

- changed: ColorExtensions: is now ColorHelper;
- changed: ListExtensions: Added methods Count(), Any(), All() and ToStringFull();
- changed: DictionaryExtensions: Added methods Count(), Any(), All() and ToStringFull();
- changed: StringHelper: Get() is now Format();
- changed: Modifiable<T>: Changed to Math folder; and created a nice Inspector editor;

### 1.1
First public version.
