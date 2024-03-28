using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]

public class ShowIfEditot : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute condHAtt = (ShowIfAttribute)attribute;
        bool enabled = GetShowIfAttributeResult(condHAtt, property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        if (!condHAtt.m_HideInInspector || enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute condHAtt = (ShowIfAttribute)attribute;
        bool enabled = GetShowIfAttributeResult(condHAtt, property);

        if (!condHAtt.m_HideInInspector || enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }

    }

    private bool GetShowIfAttributeResult(ShowIfAttribute showIfAtt, SerializedProperty property)
    {
        bool enabled = true;
        string propertyPath = property.propertyPath;

        bool need = showIfAtt.m_NeedToBe;
        bool secondNeed = showIfAtt.m_SecondNeedToBe;
        bool thirdNeed = showIfAtt.m_ThirdNeedToBe;

        bool needOneCheck = showIfAtt.m_NeedOneCHeck;
        bool needTwoCheck = showIfAtt.m_NeedTwoCheck;
        bool needThreeCheck = showIfAtt.m_NeedThreeCheck;

        string conditionPath = propertyPath.Replace(property.name, showIfAtt.m_BoolName);
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        string secondConditionPath = propertyPath.Replace(property.name, showIfAtt.m_SecondBoolName);
        SerializedProperty secondSourcePropertyValue = property.serializedObject.FindProperty(secondConditionPath);

        string thirdConditionPath = propertyPath.Replace(property.name, showIfAtt.m_ThirdBoolName);
        SerializedProperty thirdSourcePropertyValue = property.serializedObject.FindProperty(thirdConditionPath);

        if (sourcePropertyValue != null && needOneCheck)
        {
            enabled = (sourcePropertyValue.boolValue == need);
        }
        else if (secondSourcePropertyValue != null && needTwoCheck)
        {
            enabled = (sourcePropertyValue.boolValue == need && secondSourcePropertyValue.boolValue == secondNeed);
        }
        else if (thirdSourcePropertyValue != null && needThreeCheck)
        {
            enabled = (sourcePropertyValue.boolValue == need &&
                (secondSourcePropertyValue.boolValue == secondNeed && thirdSourcePropertyValue.boolValue == thirdNeed));
        }

        else
        {
            Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue has been found!");
        }

        return enabled;
    }
}
