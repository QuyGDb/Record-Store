Shader "Custom/PortalWindow"
{
    SubShader
    {
        ZWrite off           // Disables depth writing
        ColorMask 0          // Disables color output
        Cull off 
        Stencil
        {
            Ref 1            // Reference value to write
            Pass replace     // Replaces stencil buffer value with Ref on pass
        }

        Pass
        {
        }
    }
}
