#ifndef LIGHTING_CEL_SHADED_INCLUDED
#define LIGHTING_CEL_SHADED_INCLUDED

#ifndef SHADERGRAPH_PREVIEW
struct EdgeConstants {
	float shadowAttenuation;
	float distanceAttenuation;
	float diffuse;
	float specular;
	float specularOffset;
	float rim;
	float rimOffset;
};
struct SurfaceVariables {

	float3 view;
	float3 normal;
	float shininess;
	float smoothness;
	float rimThreshold;
	float shadowStep;
	EdgeConstants ec;
};

float3 CalculateCelShading(Light l, SurfaceVariables s) {
	float shadowAttenuationSmoothstepped = smoothstep(0.0f, s.ec.shadowAttenuation, l.shadowAttenuation);
	float distanceAttenuationSmoothstepped = smoothstep(0.0f, s.ec.distanceAttenuation, l.distanceAttenuation);
	float attenuation = shadowAttenuationSmoothstepped * distanceAttenuationSmoothstepped - s.shadowStep;

	float diffuse = saturate(dot(s.normal, l.direction));
	diffuse *= attenuation;
	diffuse = diffuse > 0 ? 1 : 0;

	float3 h = SafeNormalize(l.direction + s.view);
	float specular = saturate(dot(s.normal, h));
	specular = pow(specular, s.shininess);
	specular *= diffuse * s.smoothness;
	specular = specular > 0.2 ? 1 : 0;

	float rim = 1 - dot(s.view, s.normal);
	rim *= pow(diffuse, s.rimThreshold);
	rim = rim > 0.75 ? 1 : 0;

	diffuse = smoothstep(0.0f, s.ec.diffuse, diffuse);
	specular = s.smoothness * smoothstep((1 - s.smoothness) * s.ec.specular + s.ec.specularOffset, s.ec.specular + s.ec.specularOffset, specular);
	rim = s.smoothness * smoothstep(s.ec.rim - 0.5f * s.ec.rimOffset, s.ec.rim + 0.5f * s.ec.rimOffset, rim);

	return l.color * (diffuse + max(specular, rim));
}
#endif

void LightingCelShaded_float(float Smoothness, float RimThreshold,
	float3 Position, float3 Normal, float3 View, float ShadowStep,
	out float3 Color) {
#if defined(SHADERGRAPH_PREVIEW)
	...
#else
	SurfaceVariables s;
	s.normal = normalize(Normal);

	s.view = SafeNormalize(View);
	s.smoothness = Smoothness;
	s.shininess = exp2(10 * Smoothness + 1);

	s.rimThreshold = RimThreshold;

	s.shadowStep = ShadowStep;

	s.ec.shadowAttenuation = 1;
	s.ec.distanceAttenuation = 1;
	s.ec.diffuse = 1;
	s.ec.specular = 1;
	s.ec.specularOffset = 1;
	s.ec.rim = 1;
	s.ec.rimOffset = 1;

#if SHADOWS_SCREEN
	float4 clipPos = TransformWorldToHClip(Position);
	float4 shadowCoord = ComputeScreenPos(clipPos);
#else
	float4 shadowCoord = TransformWorldToShadowCoord(Position);
#endif

	Light light = GetMainLight(shadowCoord);
	Color = CalculateCelShading(light, s);

	int pixelLightCount = GetAdditionalLightsCount();
	for (int i = 0; i < pixelLightCount; i++) {
		light = GetAdditionalLight(i, Position, 1);
		Color += CalculateCelShading(light, s);
	}
#endif
}

#endif