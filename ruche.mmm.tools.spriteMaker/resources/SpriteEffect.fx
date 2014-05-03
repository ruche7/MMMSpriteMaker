//==========================================================
// Sprite effect for MikuMikuMoving
//   Generated by MMMSpriteMaker
//   Written by ruche
//==========================================================

//----------------------------------------------------------
// Config values
//----------------------------------------------------------

// 0:Sprite
// 1:Sprite (dot by dot)
// 2:Billboard
// 3:Polygon
#define SPRMAKE_CONFIG_RENDERTYPE [[ConfigRenderType]]

// 0:No
// 1:Yes
#define SPRMAKE_CONFIG_POSTEFFECT [[ConfigPostEffect]]

// 0:No
// 1:Yes
#define SPRMAKE_CONFIG_RENDERBACK [[ConfigRenderingBack]]

// 0:Disabled
// 1:Enabled
// 2:Selectable (MMM only)
#define SPRMAKE_CONFIG_LIGHT [[ConfigLightSetting]]

// 0:Disabled
// 1:Enabled
#define SPRMAKE_CONFIG_SHADOW 1

// Size per pixel (default = 0.1f)
#define SPRMAKE_CONFIG_PIXELRATIO [[ConfigPixelRatio]]

// Sprite view width (default = 64.0f)
#define SPRMAKE_CONFIG_SPRITE_VIEWWIDTH [[ConfigSpriteViewportWidth]]

// Sprite z-order range (default = 10.0f)
#define SPRMAKE_CONFIG_SPRITE_ZRANGE [[ConfigSpriteZRange]]

// 0:LeftTop
// 1:MiddleTop
// 2:RightTop
// 3:LeftMiddle
// 4:Center
// 5:RightMiddle
// 6:LeftBottom
// 7:MiddleBottom
// 8:RightBottom
#define SPRMAKE_CONFIG_BASEPOINT [[ConfigBasePoint]]

// 0:No
// 1:Yes
// 2:Selectable (MMM only)
#define SPRMAKE_CONFIG_FLIP_H [[ConfigHorizontalFlipSetting]]

// 0:No
// 1:Yes
// 2:Selectable (MMM only)
#define SPRMAKE_CONFIG_FLIP_V [[ConfigVerticalFlipSetting]]

// Ground shadow alpha clipping value (default = 0.95f)
#define SPRMAKE_CONFIG_CLIP_GROUNDSHADOW_ALPHA 0.95f

// Shadow alpha clipping value (default = 0.01f)
#define SPRMAKE_CONFIG_CLIP_SHADOW_ALPHA 0.01f

//--------------------------------------

#if SPRMAKE_CONFIG_RENDERTYPE > 2
    #define SPRMAKE_RENDER_POLYGON
#elif SPRMAKE_CONFIG_RENDERTYPE > 1
    #define SPRMAKE_RENDER_BILLBOARD
#elif SPRMAKE_CONFIG_RENDERTYPE > 0
    #define SPRMAKE_RENDER_SPRITE
    #define SPRMAKE_RENDER_DOTBYDOT
#else
    #define SPRMAKE_RENDER_SPRITE
#endif

#ifdef SPRMAKE_RENDER_SPRITE
    #undef SPRMAKE_CONFIG_LIGHT
    #define SPRMAKE_CONFIG_LIGHT 0
    #undef SPRMAKE_CONFIG_SHADOW
    #define SPRMAKE_CONFIG_SHADOW 0
#else // SPRMAKE_RENDER_SPRITE
    #undef SPRMAKE_CONFIG_POSTEFFECT
    #define SPRMAKE_CONFIG_POSTEFFECT 0
#endif // SPRMAKE_RENDER_SPRITE

//----------------------------------------------------------
// Texture atlas parameters
//----------------------------------------------------------

#define SPRMAKE_ATLAS_COUNT [[AtlasFrameCount]]
#define SPRMAKE_ATLAS_TEXWIDTH [[AtlasImageWidth]]
#define SPRMAKE_ATLAS_TEXHEIGHT [[AtlasImageHeight]]

#ifdef SPRMAKE_RENDER_DOTBYDOT
static float2 SprMake_AtlasSizeMul = { 0.1f, 0.1f };
#else
static float2 SprMake_AtlasSizeMul =
    {
        0.1f * SPRMAKE_CONFIG_PIXELRATIO,
        0.1f * SPRMAKE_CONFIG_PIXELRATIO,
    };
#endif

static float2 SprMake_AtlasSizes[SPRMAKE_ATLAS_COUNT] =
    {
[[AtlasSizes]]
    };

static float2 SprMake_AtlasBasePointMul =
    {
        -0.5f * fmod(SPRMAKE_CONFIG_BASEPOINT, 3),
        0.5f * (floor(SPRMAKE_CONFIG_BASEPOINT / 3) - 2),
    };

static float2 SprMake_AtlasPosLeftBottoms[SPRMAKE_ATLAS_COUNT] =
    {
[[AtlasPosLeftBottoms]]
    };

static float2 SprMake_AtlasUVMul =
    { 1.0f / SPRMAKE_ATLAS_TEXWIDTH, 1.0f / SPRMAKE_ATLAS_TEXHEIGHT };

static float2 SprMake_AtlasUVLeftTops[SPRMAKE_ATLAS_COUNT] =
    {
[[AtlasUVLeftTops]]
    };

static float2 SprMake_AtlasUVRightTops[SPRMAKE_ATLAS_COUNT] =
    {
[[AtlasUVRightTops]]
    };

static float2 SprMake_AtlasUVRightBottoms[SPRMAKE_ATLAS_COUNT] =
    {
[[AtlasUVRightBottoms]]
    };

static float2 SprMake_AtlasUVLeftBottoms[SPRMAKE_ATLAS_COUNT] =
    {
[[AtlasUVLeftBottoms]]
    };

//----------------------------------------------------------
// Global parameters
//----------------------------------------------------------

#if SPRMAKE_CONFIG_POSTEFFECT != 0
// for post effect
float Script : STANDARDSGLOBAL <
    string ScriptOutput = "color";
    string ScriptClass = "sceneorobject";
    string ScriptOrder = "postprocess"; > = 0.8f;
float4 ClearColor = { 0, 0, 0, 1 };
float ClearDepth = 1;
#endif // SPRMAKE_CONFIG_POSTEFFECT != 0

float4x4 WorldMatrix : WORLD;
float4x4 ViewProjMatrix : VIEWPROJECTION;
float4x4 ViewMatrix : VIEW;

#ifdef SPRMAKE_RENDER_BILLBOARD
// Matrices for billboard
float4x4 WorldViewMatrixInverse : WORLDVIEWINVERSE;
static float3x3 BillboardMatrix =
    {
        normalize(WorldViewMatrixInverse[0].xyz),
        normalize(WorldViewMatrixInverse[1].xyz),
        normalize(WorldViewMatrixInverse[2].xyz),
    };
#endif // SPRMAKE_RENDER_BILLBOARD

#ifdef SPRMAKE_RENDER_SPRITE

float2 ViewportSize : VIEWPORTPIXELSIZE;

#ifdef SPRMAKE_RENDER_DOTBYDOT

static float4x4 ProjMatrix =
    {
        2.0f / ViewportSize.x, 0,               0, 0,
        0, 2.0f / ViewportSize.y,               0, 0,
        0, 0, 1.0f / SPRMAKE_CONFIG_SPRITE_ZRANGE, 0,
        0,                     0,               0, 1,
    };

#else // SPRMAKE_RENDER_DOTBYDOT

static float ProjMatrix11 = 2.0f / SPRMAKE_CONFIG_SPRITE_VIEWWIDTH;
static float ViewportRatio = ViewportSize.x / ViewportSize.y;
static float4x4 ProjMatrix =
    {
        ProjMatrix11,                 0,        0, 0,
        0, ProjMatrix11 * ViewportRatio,        0, 0,
        0, 0, 1.0f / SPRMAKE_CONFIG_SPRITE_ZRANGE, 0,
        0,                            0,        0, 1,
    };

#endif // SPRMAKE_RENDER_DOTBYDOT
#else // SPRMAKE_RENDER_SPRITE

float4x4 ProjMatrix : PROJECTION;

#endif // SPRMAKE_RENDER_SPRITE

float3 CameraPosition : POSITION < string Object = "Camera"; >;

// Texture
texture ObjectTexture : MATERIALTEXTURE;
sampler ObjTexSampler =
    sampler_state
    {
        texture = <ObjectTexture>;
        MINFILTER = LINEAR;
        MAGFILTER = LINEAR;
#if !defined(MIKUMIKUMOVING) && defined(MME_MIPMAP)
        MIPFILTER = LINEAR;
#endif
        AddressU = BORDER;
        AddressV = BORDER;
        BorderColor = float4(0, 0, 0, 0);
    };

// Accessory
#ifdef SPRMAKE_RENDER_SPRITE
float3 AccPos : CONTROLOBJECT < string name = "(self)"; >;
#endif
float AccTrans : CONTROLOBJECT < string name = "(self)"; string item = "Tr"; >;

// Material
float4 MaterialDiffuse : DIFFUSE < string Object = "Geometry"; >;

//--------------------------------------
// For light and shadow
//--------------------------------------
#if SPRMAKE_CONFIG_LIGHT != 0 || SPRMAKE_CONFIG_SHADOW != 0
#ifdef MIKUMIKUMOVING
// for MikuMikuMoving

bool LightEnables[MMM_LightCount] : LIGHTENABLES;
float3 LightDirections[MMM_LightCount] : LIGHTDIRECTIONS;

#else // MIKUMIKUMOVING
// for MikuMikuEffect

float3 LightDirection : DIRECTION < string Object = "Light"; >;

#endif // MIKUMIKUMOVING
#endif // SPRMAKE_CONFIG_LIGHT != 0 || SPRMAKE_CONFIG_SHADOW != 0

//--------------------------------------
// For light
//--------------------------------------
#if SPRMAKE_CONFIG_LIGHT != 0

// Material
float3 MaterialAmbient : AMBIENT < string Object = "Geometry"; >;
float3 MaterialEmmisive : EMISSIVE < string Object = "Geometry"; >;
float3 MaterialSpecular : SPECULAR < string Object = "Geometry"; >;
float SpecularPower : SPECULARPOWER < string Object = "Geometry"; >;

#ifdef MIKUMIKUMOVING
// for MikuMikuMoving

// Light
float3 LightDiffuses[MMM_LightCount] : LIGHTDIFFUSECOLORS;
float3 LightAmbients[MMM_LightCount] : LIGHTAMBIENTCOLORS;
float3 LightSpeculars[MMM_LightCount] : LIGHTSPECULARCOLORS;

// Material & Light colors
static float4 DiffuseColors[3] =
    {
        MaterialDiffuse * float4(LightDiffuses[0], 1.0f),
        MaterialDiffuse * float4(LightDiffuses[1], 1.0f),
        MaterialDiffuse * float4(LightDiffuses[2], 1.0f),
    };
static float3 AmbientColors[3] =
    {
        saturate(MaterialAmbient * LightAmbients[0]) + MaterialEmmisive,
        saturate(MaterialAmbient * LightAmbients[1]) + MaterialEmmisive,
        saturate(MaterialAmbient * LightAmbients[2]) + MaterialEmmisive,
    };
static float3 SpecularColors[3] =
    {
        MaterialSpecular * LightSpeculars[0],
        MaterialSpecular * LightSpeculars[1],
        MaterialSpecular * LightSpeculars[2],
    };

#else // MIKUMIKUMOVING
// for MikuMikuEffect

// Light
float3 LightDiffuse : DIFFUSE < string Object = "Light"; >;
float3 LightAmbient : AMBIENT < string Object = "Light"; >;
float3 LightSpecular : SPECULAR < string Object = "Light"; >;

// Material & Light colors
static float4 DiffuseColor = MaterialDiffuse * float4(LightDiffuse, 1.0f);
static float3 AmbientColor = saturate(MaterialAmbient * LightAmbient + MaterialEmmisive);
static float3 SpecularColor = MaterialSpecular * LightSpecular;

#endif // MIKUMIKUMOVING
#endif // SPRMAKE_CONFIG_LIGHT != 0

//--------------------------------------
// For shadow
//--------------------------------------
#if SPRMAKE_CONFIG_SHADOW != 0

float4 MaterialToon : TOONCOLOR;
float4 GroundShadowColor : GROUNDSHADOWCOLOR;

#ifdef MIKUMIKUMOVING
// for MikuMikuMoving

float4x4 LightWVPMatrices[MMM_LightCount] : LIGHTWVPMATRICES;
float3 LightPositions[MMM_LightCount] : LIGHTPOSITIONS;
float LightZFars[MMM_LightCount] : LIGHTZFARS;

#else // MIKUMIKUMOVING
// for MikuMikuEffect

float4x4 LightWVPMatrix : WORLDVIEWPROJECTION < string Object = "Light"; >;

#endif // MIKUMIKUMOVING
#endif // SPRMAKE_CONFIG_SHADOW != 0

//----------------------------------------------------------
// Controls (for MikuMikuMoving)
//----------------------------------------------------------

#if SPRMAKE_ATLAS_COUNT >= 2
int SprMake_AtlasIndex <
    string UIName = "Index";
    string UIWidget = "Numeric";
    int UIMin = 0;
    int UIMax = SPRMAKE_ATLAS_COUNT - 1; > = 0;
#endif

#if SPRMAKE_CONFIG_LIGHT > 1
bool SprMake_LightEnabled < string UIName = "Light"; > = true;
#endif

#if SPRMAKE_CONFIG_FLIP_H > 1
bool SprMake_FlipHorz < string UIName = "Flip_H"; > = false;
#endif

#if SPRMAKE_CONFIG_FLIP_V > 1
bool SprMake_FlipVert < string UIName = "Flip_V"; > = false;
#endif

//----------------------------------------------------------
// Common functions
//----------------------------------------------------------

// Texture atlas info
struct SPRMAKE_ATLAS_INFO
{
    float2 Size;
    float2 PosLeftBottom;
    float2 UVLeftTop;
    float2 UVRightTop;
    float2 UVRightBottom;
    float2 UVLeftBottom;
};

//--------------------------------------
// Get texture atlas info
//--------------------------------------
SPRMAKE_ATLAS_INFO SprMake_GetAtlasInfo()
{
    SPRMAKE_ATLAS_INFO Out = (SPRMAKE_ATLAS_INFO)0;

    // select atlas
[[SelectAtlasCode]]

#if SPRMAKE_CONFIG_FLIP_H != 0 || SPRMAKE_CONFIG_FLIP_V != 0
    float2 uvSwapTemp;
#endif

    // flip horizontal
#if SPRMAKE_CONFIG_FLIP_H != 0
#if SPRMAKE_CONFIG_FLIP_H > 1
    if (SprMake_FlipHorz)
    {
#endif // SPRMAKE_CONFIG_FLIP_H > 1
        uvSwapTemp = Out.UVLeftTop;
        Out.UVLeftTop = Out.UVRightTop;
        Out.UVRightTop = uvSwapTemp;
        uvSwapTemp = Out.UVLeftBottom;
        Out.UVLeftBottom = Out.UVRightBottom;
        Out.UVRightBottom = uvSwapTemp;
#if SPRMAKE_CONFIG_FLIP_H > 1
    }
#endif // SPRMAKE_CONFIG_FLIP_H > 1
#endif // SPRMAKE_CONFIG_FLIP_H != 0

    // flip vertical
#if SPRMAKE_CONFIG_FLIP_V != 0
#if SPRMAKE_CONFIG_FLIP_V > 1
    if (SprMake_FlipVert)
    {
#endif // SPRMAKE_CONFIG_FLIP_V > 1
        uvSwapTemp = Out.UVLeftTop;
        Out.UVLeftTop = Out.UVLeftBottom;
        Out.UVLeftBottom = uvSwapTemp;
        uvSwapTemp = Out.UVRightTop;
        Out.UVRightTop = Out.UVRightBottom;
        Out.UVRightBottom = uvSwapTemp;
#if SPRMAKE_CONFIG_FLIP_V > 1
    }
#endif // SPRMAKE_CONFIG_FLIP_V > 1
#endif // SPRMAKE_CONFIG_FLIP_V != 0

    return Out;
}

//--------------------------------------
// Calculate position
//--------------------------------------
float4 SprMake_CalcPosition(float4 Pos, float2 AtlasPosLeftBottom, float2 AtlasSize)
{
    float4 Out = Pos;

    Out.xy = AtlasPosLeftBottom + (Out.xy * AtlasSize);

#ifdef SPRMAKE_RENDER_BILLBOARD
    Out.xyz = mul(Out.xyz, BillboardMatrix);
#endif // SPRMAKE_RENDER_BILLBOARD

    return Out;
}

//--------------------------------------
// Calculate world position
//--------------------------------------
float4 SprMake_CalcWorldPosition(
    float4 Pos,
    float2 AtlasPosLeftBottom,
    float2 AtlasSize,
    uniform float4x4 WorldMat)
{
    return mul(SprMake_CalcPosition(Pos, AtlasPosLeftBottom, AtlasSize), WorldMat);
}

//--------------------------------------
// Calculate vertex position
//--------------------------------------
float4 SprMake_CalcVertexPosition(
    float4 WorldPos,
    uniform float4x4 ViewMat,
    uniform float4x4 ProjMat)
{
#ifdef SPRMAKE_RENDER_SPRITE

    float4 Out = mul(WorldPos, ProjMat);

#else // SPRMAKE_RENDER_SPRITE
#ifdef MIKUMIKUMOVING
    // for MikuMikuMoving

    float4 Out;
    if (MMM_IsDinamicProjection)
    {
        float3 eye = CameraPosition - WorldPos.xyz;
        Out = mul(WorldPos, mul(ViewMat, MMM_DynamicFov(ProjMat, length(eye))));
    }
    else
    {
        Out = mul(WorldPos, mul(ViewMat, ProjMat));
    }

#else // MIKUMIKUMOVING
    // for MikuMikuEffect

    float4 Out = mul(WorldPos, mul(ViewMat, ProjMat));

#endif // MIKUMIKUMOVING
#endif // SPRMAKE_RENDER_SPRITE

    return Out;
}

//--------------------------------------
// Calculate texture coord
//--------------------------------------
float2 SprMake_CalcTexCoord(
    float2 Tex,
    float2 AtlasUVLeftTop,
    float2 AtlasUVRightTop,
    float2 AtlasUVRightBottom,
    float2 AtlasUVLeftBottom)
{
    return
        lerp(
            lerp(AtlasUVLeftTop, AtlasUVRightTop, Tex.x),
            lerp(AtlasUVLeftBottom, AtlasUVRightBottom, Tex.x),
            Tex.y);
}

#if SPRMAKE_CONFIG_SHADOW != 0 && defined(MIKUMIKUMOVING)
// for MikuMikuMoving

// Active light info
struct SPRMAKE_LIGHT_INFO
{
    float4x4 WVPMatrix;
    float3 Pos;
    float ZFar;
};

//--------------------------------------
// Check self-shadow
//--------------------------------------
bool SprMake_IsSelfShadowEnabled()
{
    int count = int(LightEnables[0]) + int(LightEnables[1]) + int(LightEnables[2]);

    // multi-lighting is not supported
    return (count == 1);
}

//--------------------------------------
// Get active light info
//--------------------------------------
SPRMAKE_LIGHT_INFO SprMake_GetActiveLightInfo()
{
    SPRMAKE_LIGHT_INFO Out = (SPRMAKE_LIGHT_INFO)0;

    for (int i = 0; i < 3; ++i)
    {
        if (LightEnables[i])
        {
            Out.WVPMatrix = LightWVPMatrices[i];
            Out.Pos = LightPositions[i];
            Out.ZFar = LightZFars[i];
            break;
        }
    }

    return Out;
}

#endif // SPRMAKE_CONFIG_SHADOW != 0 && defined(MIKUMIKUMOVING)

//----------------------------------------------------------
// Shader for object
//----------------------------------------------------------

// Vertex shader output for object
struct VS_OUTPUT
{
    float4 Pos : POSITION;
    float2 Tex : TEXCOORD0;
    float3 Normal : TEXCOORD2;
    float4 Color : COLOR0;

#if SPRMAKE_CONFIG_LIGHT != 0
    float3 Eye : TEXCOORD3;
#endif // SPRMAKE_CONFIG_LIGHT != 0

#if SPRMAKE_CONFIG_SHADOW != 0 && defined(MIKUMIKUMOVING)
    float4 SS_UV1 : TEXCOORD4;
    float4 SS_UV2 : TEXCOORD5;
    float4 SS_UV3 : TEXCOORD6;
#endif // SPRMAKE_CONFIG_SHADOW != 0 && defined(MIKUMIKUMOVING)
};

//--------------------------------------
// Vertex shader for object
//--------------------------------------
VS_OUTPUT SprMake_VS(
    float4 Pos : POSITION,
    float3 Normal : NORMAL,
    float2 Tex : TEXCOORD0,
    uniform bool useSelfShadow)
{
    VS_OUTPUT Out = (VS_OUTPUT)0;

    static const SPRMAKE_ATLAS_INFO atlasInfo = SprMake_GetAtlasInfo();

    float4 wpos =
        SprMake_CalcWorldPosition(
            Pos,
            atlasInfo.PosLeftBottom,
            atlasInfo.Size,
            WorldMatrix);
    Out.Pos = SprMake_CalcVertexPosition(wpos, ViewMatrix, ProjMatrix);

    Out.Normal = normalize(mul(Normal, (float3x3)WorldMatrix));

    Out.Tex =
        SprMake_CalcTexCoord(
            Tex,
            atlasInfo.UVLeftTop,
            atlasInfo.UVRightTop,
            atlasInfo.UVRightBottom,
            atlasInfo.UVLeftBottom);

    Out.Color = float4(1, 1, 1, 1);

    // Light ---->
#if SPRMAKE_CONFIG_LIGHT != 0

    Out.Eye = CameraPosition - wpos.xyz;

#if SPRMAKE_CONFIG_LIGHT > 1
    if (SprMake_LightEnabled)
    {
#endif // SPRMAKE_CONFIG_LIGHT > 1
#ifdef MIKUMIKUMOVING
        // for MikuMikuMoving

        float3 color = float3(0, 0, 0);
        float3 ambient = float3(0, 0, 0);
        float count = 0;
        for (int i = 0; i < 3; ++i)
        {
            if (LightEnables[i])
            {
                color += (float3(1, 1, 1) - color) * (max(0, DiffuseColors[i] * dot(Out.Normal, -LightDirections[i])));
                ambient += AmbientColors[i];
                count = count + 1.0;
            }
        }
        Out.Color.rgb = saturate(ambient / count + color);

#else // MIKUMIKUMOVING
        // for MikuMikuEffect

        Out.Color.rgb = AmbientColor;
        Out.Color.rgb += max(0, dot(Out.Normal, -LightDirection)) * DiffuseColor.rgb;
        Out.Color.rgb = saturate(Out.Color.rgb);

#endif // MIKUMIKUMOVING
#if SPRMAKE_CONFIG_LIGHT > 1
    }
#endif // SPRMAKE_CONFIG_LIGHT > 1

#endif // SPRMAKE_CONFIG_LIGHT != 0
    // <---- Light

    // Shadow ---->
#if SPRMAKE_CONFIG_SHADOW != 0 && defined(MIKUMIKUMOVING)

    if (useSelfShadow)
    {
        Out.SS_UV1 = mul(wpos, LightWVPMatrices[0]);
        Out.SS_UV2 = mul(wpos, LightWVPMatrices[1]);
        Out.SS_UV3 = mul(wpos, LightWVPMatrices[2]);
        Out.SS_UV1.y = -Out.SS_UV1.y;
        Out.SS_UV2.y = -Out.SS_UV2.y;
        Out.SS_UV3.y = -Out.SS_UV3.y;
        Out.SS_UV1.z = (length(LightPositions[0] - wpos.xyz) / LightZFars[0]);
        Out.SS_UV2.z = (length(LightPositions[1] - wpos.xyz) / LightZFars[1]);
        Out.SS_UV3.z = (length(LightPositions[2] - wpos.xyz) / LightZFars[2]);
    }

#endif // SPRMAKE_CONFIG_SHADOW != 0 && defined(MIKUMIKUMOVING)
    // <---- Shadow

    Out.Color.a = MaterialDiffuse.a;

    return Out;
}

//--------------------------------------
// Pixel shader for object
//--------------------------------------
float4 SprMake_PS(VS_OUTPUT IN, uniform bool useSelfShadow) : COLOR0
{
    float4 Out = IN.Color * tex2D(ObjTexSampler, IN.Tex);

    // Shadow ---->
#if SPRMAKE_CONFIG_SHADOW != 0
#ifdef MIKUMIKUMOVING
    // for MikuMikuMoving

    if (useSelfShadow)
    {
        Out.rgb *= MMM_GetSelfShadowToonColor(MaterialToon, IN.Normal, IN.SS_UV1, IN.SS_UV2, IN.SS_UV3, false, false);
    }

#else // MIKUMIKUMOVING
    // for MikuMikuEffect

    // TODO:

#endif // MIKUMIKUMOVING
#endif // SPRMAKE_CONFIG_SHADOW != 0
    // <-- Shadow

    // Light ---->
#if SPRMAKE_CONFIG_LIGHT != 0
#ifdef MIKUMIKUMOVING
    // for MikuMikuMoving

#if SPRMAKE_CONFIG_LIGHT > 1
    if (SprMake_LightEnabled)
    {
#endif // SPRMAKE_CONFIG_LIGHT > 1
        float3 specular = 0;
        float3 halfVector;
        for (int i = 0; i < 3; ++i)
        {
            if (LightEnables[i])
            {
                halfVector = normalize(normalize(IN.Eye) + -LightDirections[i]);
                specular += pow(max(0, dot(halfVector, normalize(IN.Normal))), SpecularPower) * SpecularColors[i];
            }
        }
        Out.rgb += specular;
#if SPRMAKE_CONFIG_LIGHT > 1
    }
#endif // SPRMAKE_CONFIG_LIGHT > 1

#else // MIKUMIKUMOVING
    // for MikuMikuEffect

    // TODO:

#endif // MIKUMIKUMOVING
#endif // SPRMAKE_CONFIG_LIGHT != 0
    // <---- Light

    Out.a *= AccTrans;

    return Out;
}

//--------------------------------------
// Technique for object without SelfShadow
//--------------------------------------
technique MainTec <
    string MMDPass = "object";
    bool UseTexture = true;
    bool UseToon = false;
#ifdef MIKUMIKUMOVING
    bool UseSelfShadow = false;
#endif // MIKUMIKUMOVING
#if SPRMAKE_CONFIG_POSTEFFECT != 0
    string Script =
        "RenderColorTarget0=;"
        "RenderDepthStencilTarget=;"
        "ClearSetColor=ClearColor;"
        "ClearSetDepth=ClearDepth;"
        "Clear=Color;"
        "Clear=Depth;"
        "ScriptExternal=Color;"
        "Pass=DrawObject;";
#endif // SPRMAKE_CONFIG_POSTEFFECT != 0
    >
{
    pass DrawObject
    {
#if SPRMAKE_CONFIG_RENDERBACK != 0
        CullMode = NONE;
#endif // SPRMAKE_CONFIG_RENDERBACK != 0

        VertexShader = compile vs_3_0 SprMake_VS(false);
        PixelShader  = compile ps_3_0 SprMake_PS(false);
    }
}

//--------------------------------------
// Technique for object with SelfShadow
//--------------------------------------
technique MainTecSS <
#ifdef MIKUMIKUMOVING
    string MMDPass = "object";
    bool UseSelfShadow = true;
#else // MIKUMIKUMOVING
    string MMDPass = "object_ss";
#endif // MIKUMIKUMOVING
    bool UseTexture = true;
    bool UseToon = false;
#if SPRMAKE_CONFIG_POSTEFFECT != 0
    string Script =
        "RenderColorTarget0=;"
        "RenderDepthStencilTarget=;"
        "ClearSetColor=ClearColor;"
        "ClearSetDepth=ClearDepth;"
        "Clear=Color;"
        "Clear=Depth;"
        "ScriptExternal=Color;"
        "Pass=DrawObject;";
#endif // SPRMAKE_CONFIG_POSTEFFECT != 0
    >
{
    pass DrawObject
    {
#if SPRMAKE_CONFIG_RENDERBACK != 0
        CullMode = NONE;
#endif // SPRMAKE_CONFIG_RENDERBACK != 0

        VertexShader = compile vs_3_0 SprMake_VS(true);
        PixelShader  = compile ps_3_0 SprMake_PS(true);
    }
}

//----------------------------------------------------------
// Shader for shadow
//----------------------------------------------------------

#if SPRMAKE_CONFIG_SHADOW != 0

// Vertex shader output for shadow
struct VS_SHADOW_OUTPUT
{
    float4 Pos : POSITION;
    float2 Tex : TEXCOORD0;
};

//--------------------------------------
// Vertex shader for shadow
//--------------------------------------
VS_SHADOW_OUTPUT SprMake_ShadowVS(float4 Pos : POSITION, uniform int lightIndex)
{
    VS_SHADOW_OUTPUT Out = (VS_SHADOW_OUTPUT)0;

    static const SPRMAKE_ATLAS_INFO atlasInfo = SprMake_GetAtlasInfo();

#ifdef MIKUMIKUMOVING
    if (!LightEnables[lightIndex])
    {
        return Out;
    }

    float3 lightDir = LightDirections[lightIndex];
#else
    uniform float3 lightDir = LightDirection;
#endif

    float4 wpos =
        SprMake_CalcWorldPosition(
            Pos,
            atlasInfo.PosLeftBottom,
            atlasInfo.Size,
            WorldMatrix);
    wpos.xyz -= lightDir * ((lightDir.y == 0) ? -999999.9f : (wpos.y / lightDir.y));
    wpos.y += 0.02f;
    Out.Pos = SprMake_CalcVertexPosition(wpos, ViewMatrix, ProjMatrix);

    Out.Tex =
        SprMake_CalcTexCoord(
            float2(Pos.x, 1 - Pos.y),
            atlasInfo.UVLeftTop,
            atlasInfo.UVRightTop,
            atlasInfo.UVRightBottom,
            atlasInfo.UVLeftBottom);

    return Out;
}

//--------------------------------------
// Pixel shader for shadow
//--------------------------------------
float4 SprMake_ShadowPS(float2 Tex : TEXCOORD0, uniform int lightIndex) : COLOR
{
#ifdef MIKUMIKUMOVING
    clip(-float(!LightEnables[lightIndex]));
#endif // MIKUMIKUMOVING

    float4 Out = float4(0, 0, 0, 0);

    float texAlpha = tex2D(ObjTexSampler, Tex).a;
    clip(texAlpha - SPRMAKE_CONFIG_CLIP_GROUNDSHADOW_ALPHA);

    Out = GroundShadowColor;
    Out.a *= texAlpha * AccTrans;
    clip(Out.a - SPRMAKE_CONFIG_CLIP_SHADOW_ALPHA);

    return Out;
}

#endif // SPRMAKE_CONFIG_SHADOW != 0

//--------------------------------------
// Technique for shadow
//--------------------------------------
technique ShadowTec < string MMDPass = "shadow"; bool UseToon = false; >
{
#if SPRMAKE_CONFIG_SHADOW != 0

    pass DrawShadow0
    {
#if SPRMAKE_CONFIG_RENDERBACK != 0
        CullMode = NONE;
#endif // SPRMAKE_CONFIG_RENDERBACK != 0

        VertexShader = compile vs_3_0 SprMake_ShadowVS(0);
        PixelShader  = compile ps_3_0 SprMake_ShadowPS(0);
    }

#ifdef MIKUMIKUMOVING

    pass DrawShadow1
    {
#if SPRMAKE_CONFIG_RENDERBACK != 0
        CullMode = NONE;
#endif // SPRMAKE_CONFIG_RENDERBACK != 0

        VertexShader = compile vs_3_0 SprMake_ShadowVS(1);
        PixelShader  = compile ps_3_0 SprMake_ShadowPS(1);
    }

    pass DrawShadow2
    {
#if SPRMAKE_CONFIG_RENDERBACK != 0
        CullMode = NONE;
#endif // SPRMAKE_CONFIG_RENDERBACK != 0

        VertexShader = compile vs_3_0 SprMake_ShadowVS(2);
        PixelShader  = compile ps_3_0 SprMake_ShadowPS(2);
    }

#endif // MIKUMIKUMOVING
#endif // SPRMAKE_CONFIG_SHADOW != 0
}

//----------------------------------------------------------
// Shader for zplot
//----------------------------------------------------------

#if SPRMAKE_CONFIG_SHADOW != 0

// Vertex shader output for zplot
struct VS_ZPLOT_OUTPUT
{
    float4 Pos : POSITION;
    float2 Tex : TEXCOORD0;
    float4 ShadowMapTex : TEXCOORD1;
};

//--------------------------------------
// Vertex shader for zplot
//--------------------------------------
VS_ZPLOT_OUTPUT SprMake_ZplotVS(float4 Pos : POSITION)
{
    VS_ZPLOT_OUTPUT Out = (VS_ZPLOT_OUTPUT)0;

    static const SPRMAKE_ATLAS_INFO atlasInfo = SprMake_GetAtlasInfo();

#ifdef MIKUMIKUMOVING
    // for MikuMikuMoving

    static const bool selfShadowDisabled = !SprMake_IsSelfShadowEnabled();

    if (selfShadowDisabled)
    {
        return Out;
    }

    static const SPRMAKE_LIGHT_INFO lightInfo = SprMake_GetActiveLightInfo();

    float4 wpos =
        SprMake_CalcWorldPosition(
            Pos,
            atlasInfo.PosLeftBottom,
            atlasInfo.Size,
            WorldMatrix);
    Out.Pos = mul(wpos, lightInfo.WVPMatrix);

    Out.ShadowMapTex = Out.Pos;
    Out.ShadowMapTex.y = -Out.Pos.y;
    Out.ShadowMapTex.z = length(lightInfo.Pos - wpos.xyz) / lightInfo.ZFar;

#else // MIKUMIKUMOVING
    // for MikuMikuEffect

    float4 bpos = SprMake_CalcPosition(Pos, atlasInfo.PosLeftBottom, atlasInfo.Size);
    Out.Pos = mul(bpos, LightWVPMatrix);

    Out.ShadowMapTex = Out.Pos;

#endif // MIKUMIKUMOVING

    Out.Tex =
        SprMake_CalcTexCoord(
            float2(Pos.x, 1 - Pos.y),
            atlasInfo.UVLeftTop,
            atlasInfo.UVRightTop,
            atlasInfo.UVRightBottom,
            atlasInfo.UVLeftBottom);

    return Out;
}

//--------------------------------------
// Pixel shader for zplot
//--------------------------------------
float4 SprMake_ZplotPS(float2 Tex : TEXCOORD0, float4 ShadowMapTex : TEXCOORD1) : COLOR
{
#ifdef MIKUMIKUMOVING
    static const bool selfShadowDisabled = !SprMake_IsSelfShadowEnabled();
    clip(-float(selfShadowDisabled));
#endif // MIKUMIKUMOVING

    float alpha = tex2D(ObjTexSampler, Tex).a * AccTrans;
    clip(alpha - SPRMAKE_CONFIG_CLIP_SHADOW_ALPHA);

    return float4(ShadowMapTex.z / ShadowMapTex.w, 0, 0, 1);
}

#endif // SPRMAKE_CONFIG_SHADOW != 0

//--------------------------------------
// Technique for zplot
//--------------------------------------

technique ZplotTec < string MMDPass = "zplot"; bool UseToon = false; >
{
#if SPRMAKE_CONFIG_SHADOW != 0

    pass DrawZplot0
    {
#if SPRMAKE_CONFIG_RENDERBACK != 0
        CullMode = NONE;
#endif // SPRMAKE_CONFIG_RENDERBACK != 0
        AlphaBlendEnable = FALSE;

        VertexShader = compile vs_3_0 SprMake_ZplotVS();
        PixelShader  = compile ps_3_0 SprMake_ZplotPS();
    }

#endif // SPRMAKE_CONFIG_SHADOW != 0
}

//----------------------------------------------------------

technique EdgeTec < string MMDPass = "edge"; bool UseToon = false; > { }
