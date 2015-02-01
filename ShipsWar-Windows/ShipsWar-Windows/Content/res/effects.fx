struct VertexToPixel
{
    float4 Position   	: POSITION;    
    float4 Color		: COLOR0;
    float LightingFactor: TEXCOORD0;
    float2 TextureCoords: TEXCOORD1;
};

struct PixelToFrame
{
    float4 Color : COLOR0;
};

//------- Constants --------
float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float3 xLightDirection;
float xAmbient;
bool xEnableLighting;
bool xShowNormals;
float4 xGlowColor     = float4(0.3f, 0.3f, 0.3f, 1.0f);
float4 xGlowAmbient     = float4(0.1f, 0.1f, 0.1f, 1.0f);


//------- Texture Samplers --------

Texture xTexture;
sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

//------- Technique: Pretransformed --------

VertexToPixel PretransformedVS( float4 inPos : POSITION, float4 inColor: COLOR)
{	
	VertexToPixel Output = (VertexToPixel)0;
	
	Output.Position = inPos;
	Output.Color = inColor;
    
	return Output;    
}

PixelToFrame PretransformedPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
	
	Output.Color = PSIn.Color;

	return Output;
}

technique Pretransformed
{
	pass Pass0
    {   
    	VertexShader = compile vs_1_1 PretransformedVS();
        PixelShader  = compile ps_1_1 PretransformedPS();
    }
}

//------- Technique: Colored --------

float4 xColor;

VertexToPixel ColoredVS( float4 inPos : POSITION, float4 inColor: COLOR, float3 inNormal: NORMAL)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	Output.Position = mul(inPos, preWorldViewProjection);
	Output.Color = xColor;//inColor;
	
	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	
	Output.LightingFactor = 1;
	if (xEnableLighting)
		Output.LightingFactor = dot(Normal, -xLightDirection);
    
	return Output;    
}

PixelToFrame ColoredPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
    
	Output.Color = PSIn.Color*clamp(PSIn.LightingFactor + xAmbient,0,1);
	
	return Output;
}

technique Colored
{
	pass Pass0
    {   
		AlphaBlendEnable = FALSE;
    	
    	VertexShader = compile vs_1_1 ColoredVS();
        PixelShader  = compile ps_1_1 ColoredPS();
    }
}

//------- Technique: Textured --------

VertexToPixel TexturedVS( float4 inPos : POSITION, float3 inNormal: NORMAL, float2 inTexCoords: TEXCOORD0)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	Output.Position = mul(inPos, preWorldViewProjection);	
	Output.TextureCoords = inTexCoords;
	
	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	
	Output.LightingFactor = 1;
	if (xEnableLighting)
		Output.LightingFactor = dot(Normal, -xLightDirection);
    
	return Output;    
}

PixelToFrame TexturedPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
	
	Output.Color = tex2D(TextureSampler, PSIn.TextureCoords)*clamp(PSIn.LightingFactor + xAmbient,0,1);

	return Output;
}


PixelToFrame TexturedColorPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
	
	Output.Color = xGlowColor * tex2D(TextureSampler, PSIn.TextureCoords)*clamp(PSIn.LightingFactor + xAmbient,0,1);

	return Output;
}


// BLUR/GLOW



VertexToPixel GlowVS( float4 inPos : POSITION, float3 inNormal: NORMAL, float2 inTexCoords: TEXCOORD0)
{	
	VertexToPixel Output = (VertexToPixel)0;
	
	float4x4 preWorldViewProjection = mul(xWorld, xView);
	
	float3 N = normalize(mul(inNormal, (float3x3)preWorldViewProjection));     // normal (view space)
	float3 P = mul(inPos, preWorldViewProjection) + 1.5 * N;    // displaced position (view space)  
	float3 A = float3(0, 0, 1);                                 // glow axis
    float Power;
    Power  = dot(N, A);
    Power *= Power;
    Power -= 1;
    Power *= Power;     // Power = (1 - (N.A)^2)^2 [ = ((N.A)^2 - 1)^2 ]
    Output.Position = mul(float4(P, 1), xProjection);   // projected position

	
	
	//float4x4 preViewProjection = mul (xView, xProjection);
	//float4x4 preWorldViewProjection = mul (xWorld, preViewProjection)
	//Output.Position = mul(inPos, preWorldViewProjection);	
	
	Output.Color = (xGlowColor * Power) + xGlowAmbient; // modulated glow color + glow ambient
	//Output.Color.a = 1;
	
	Output.TextureCoords = inTexCoords;
	
	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	
	Output.LightingFactor = 1;
	if (xEnableLighting)
		Output.LightingFactor = dot(Normal, -xLightDirection);
    
	return Output;    
}

PixelToFrame GlowPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
	
	Output.Color = PSIn.Color;
	//Output.Color = xGlowColor;//tex2D(TextureSampler, PSIn.TextureCoords)*clamp(PSIn.LightingFactor + xAmbient,0,1);
	//Output.Color.a = 0.0;
	
	return Output;
}


technique Textured
{
	pass Pass1
    {
        AlphaBlendEnable = FALSE;
		//AlphaBlendEnable = TRUE;
		//SrcBlend         = SrcAlpha;
        //DestBlend        = SrcAlpha;
   
    	VertexShader = compile vs_1_1 TexturedVS();
        PixelShader  = compile ps_1_1 TexturedPS();
    }  
}


technique TexturedColor
{
	pass Pass1
    {
        AlphaBlendEnable = FALSE;
		//AlphaBlendEnable = TRUE;
		//SrcBlend         = SrcAlpha;
        //DestBlend        = SrcAlpha;
   
    	VertexShader = compile vs_1_1 TexturedVS();
        PixelShader  = compile ps_1_1 TexturedColorPS();
    }
    
    
    
}


technique GlowTextured
{
	
	
	
    pass Pass1
    {
        AlphaBlendEnable = FALSE;
		//AlphaBlendEnable = TRUE;
		//SrcBlend         = SrcAlpha;
        //DestBlend        = SrcAlpha;
   
    	VertexShader = compile vs_1_1 TexturedVS();
        PixelShader  = compile ps_1_1 TexturedPS();
    }
    pass Pass0
    {
            // enable alpha blending
        AlphaBlendEnable = TRUE;
        SrcBlend         = ONE;
        DestBlend        = ONE;
   
    	VertexShader = compile vs_1_1 GlowVS();
        PixelShader  = compile ps_1_1 GlowPS();
    }
}

//------- Technique: PointSprites --------

struct SpritesVertexToPixel
{
    float4 Position   	: POSITION;
    float4 Color    	: COLOR0;
    float1 Size 		: PSIZE;
};

SpritesVertexToPixel PointSpritesVS (float4 Position : POSITION, float4 Color : COLOR0, float1 Size : PSIZE)
{
    SpritesVertexToPixel Output = (SpritesVertexToPixel)0;
     
    float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection); 
    Output.Position = mul(Position, preWorldViewProjection);    
    Output.Color = Color;
    Output.Size = 1/(pow(Output.Position.z,2)+1	) * Size;
    
    return Output;    
}

PixelToFrame PointSpritesPS(SpritesVertexToPixel PSIn, float2 TexCoords  : TEXCOORD0)
{ 
    PixelToFrame Output = (PixelToFrame)0;

    Output.Color = tex2D(TextureSampler, TexCoords);
    
    return Output;
}

technique PointSprites
{
	pass Pass0
    {   
    	PointSpriteEnable = true;
    	VertexShader = compile vs_1_1 PointSpritesVS();
        PixelShader  = compile ps_1_1 PointSpritesPS();
    }
}

