using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

public class PipelineAndCameraSwitcherAttributeProcessor : OdinAttributeProcessor<PipelineAndCameraSwitcher>
{
    public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
    {
        switch (member.Name)
        {
            case "backGroundColor":
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new BoxGroupAttribute("Cameras"));
                attributes.Add(new PropertyOrderAttribute(100));
                attributes.Add(new OnValueChangedAttribute("OnColorChanged"));
                break;
            case "pipelineAssets":
                attributes.Add(new LabelTextAttribute(" RenderPipelines"));
                attributes.Add(new ListDrawerSettingsAttribute()
                {
                    OnBeginListElementGUI = "BeginPipelines",
                    OnEndListElementGUI = "EndPipelines",
                    Expanded = true,
                    DraggableItems = false,
                });
                break;
            case "cameras":
                attributes.Add(new LabelTextAttribute(" Cameras"));
                attributes.Add(new ListDrawerSettingsAttribute()
                {
                    OnBeginListElementGUI = "BeginCameras",
                    OnEndListElementGUI = "EndCameras",
                    Expanded = true,
                    DraggableItems = false,
                });
                break;
        }
    }
}
