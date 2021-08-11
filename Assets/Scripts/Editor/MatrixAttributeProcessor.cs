using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Editor
{
    public class MatrixAttributeProcessor : OdinAttributeProcessor<MatrixRenderers>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            attributes.Add(new LabelWidthAttribute(80f));
            switch (member.Name)
            {
                case "prefab":
                    attributes.Add(new ShowIfAttribute("$enable"));
                    attributes.Add(new HideLabelAttribute());
                    attributes.Add(new AssetsOnlyAttribute());
                    break;
                case "lines":
                    attributes.Add(new LabelTextAttribute(" "));
                    attributes.Add(new PropertyOrderAttribute(-100));
                    attributes.Add(new ListDrawerSettingsAttribute());
                    break;
                case "prefabRoot":
                    attributes.Add(new BoxGroupAttribute("Prefab"));
                    break;
                case "euler":
                    attributes.Add(new BoxGroupAttribute("Prefab"));
                    attributes.Add(new OnValueChangedAttribute("OnEulerChanged"));
                    break;
                case "interval1":
                case "interval2":
                    attributes.Add(new BoxGroupAttribute("Prefab"));
                    attributes.Add(new OnValueChangedAttribute("OnIntervalChanged"));
                    break;
                case "rootPos":
                    attributes.Add(new BoxGroupAttribute("Root"));
                    attributes.Add(new OnValueChangedAttribute("OnRootPosChanged"));
                    break;
                case "rootScale":
                    attributes.Add(new BoxGroupAttribute("Root"));
                    attributes.Add(new OnValueChangedAttribute("OnScaleChanged"));
                    break;
                case "rootEuler":
                    attributes.Add(new BoxGroupAttribute("Root"));
                    attributes.Add(new OnValueChangedAttribute("OnRootEulerChanged"));
                    break;
                case "ToggleEnable":
                    attributes.Add(new PropertyOrderAttribute(-200));
                    attributes.Add(new ButtonAttribute());
                    break;
                case "ReCreate":
                    attributes.Add(new ButtonAttribute(ButtonSizes.Medium));
                    break;
            }
        }
    }
}