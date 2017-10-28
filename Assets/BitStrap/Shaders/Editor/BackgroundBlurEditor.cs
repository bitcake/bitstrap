using UnityEditor;

public class BackgroundBlurEditor : MaterialEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if( isVisible )
        {
            EditorGUI.BeginChangeCheck();

            DrawShaderProperty( "_Color" );
            DrawShaderProperty( "_BlurAmount" );
            DrawShaderProperty( "_DesaturationAmount" );

            if( EditorGUI.EndChangeCheck() )
                PropertiesChanged();
        }
    }

    private void DrawShaderProperty( string propertyName )
    {
        var property = GetMaterialProperty( targets, propertyName );
        ShaderProperty( property, property.displayName );
    }
}
