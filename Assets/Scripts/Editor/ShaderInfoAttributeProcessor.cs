using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector.Editor;

namespace Editor
{
    public class ShaderInfoAttributeProcessor : OdinAttributeProcessor<CustomShaderInfo>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
        }
    }
}