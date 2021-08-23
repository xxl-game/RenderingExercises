using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

public class Matrix4x4AttributeProcessor : OdinAttributeProcessor<Matrix4x4>
{
    public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
    {
        attributes.Add(new InlinePropertyAttribute());
        attributes.Add(new HideLabelAttribute());
    }

    public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
    {
        if (member is FieldInfo)
        {
            attributes.Add(new HideLabelAttribute());
            attributes.Add(new IndentAttribute(-1));
            attributes.Add(new LabelWidthAttribute(30));
            var first = member.Name.Substring(1, 1);
            var second = member.Name.Substring(2, 1);
            attributes.Add(new HorizontalGroupAttribute(first));
            if (first != "3")
            {
                if (second == "3")
                {
                    attributes.Add(new GUIColorAttribute(.8f, 1f, 1f));
                }
                else
                {
                    attributes.Add(new GUIColorAttribute(.7f, .9f, 1f));
                }
            }
        }

        if (member.Name == "determinant")
        {
            attributes.Add(new IndentAttribute(-1));
            attributes.Add(new HideLabelAttribute());
            attributes.Add(new SuffixLabelAttribute("determinant"));
            attributes.Add(new ShowInInspectorAttribute());
        }
    }
}