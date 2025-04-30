// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
#if GLES
using Silk.NET.OpenGLES;
#elif GL
using Silk.NET.OpenGL;
#elif LEGACY
using Silk.NET.OpenGL.Legacy;
#endif

#if GL
namespace Silk.NET.OpenGL.Extensions.ImGui
#elif GLES
namespace Silk.NET.OpenGLES.Extensions.ImGui
#elif LEGACY
namespace Silk.NET.OpenGL.Legacy.Extensions.ImGui
#endif
{
    public enum TextureCoordinate
    {
        S = TextureParameterName.TextureWrapS,
        T = TextureParameterName.TextureWrapT,
        R = TextureParameterName.TextureWrapR
    }

    class Texture : IDisposable
    {
        public const SizedInternalFormat Srgb8Alpha8 = (SizedInternalFormat) GLEnum.Srgb8Alpha8;
        public const SizedInternalFormat Rgb32F = (SizedInternalFormat) GLEnum.Rgb32f;

        public const GLEnum MaxTextureMaxAnisotropy = (GLEnum) 0x84FF;

        public static float? MaxAniso;
        private readonly GL _gl;
        public readonly string Name = null;
        public readonly uint GlTexture;
        public readonly uint Width, Height;
        public readonly uint MipmapLevels;
        public readonly SizedInternalFormat InternalFormat;

        public unsafe Texture(GL gl, int width, int height, IntPtr data, bool generateMipmaps = false, bool srgb = false)
        {
            _gl = gl;
            MaxAniso ??= gl.GetFloat(MaxTextureMaxAnisotropy);
            Width = (uint) width;
            Height = (uint) height;
            InternalFormat = srgb ? Srgb8Alpha8 : SizedInternalFormat.Rgba8;
            MipmapLevels = (uint) (generateMipmaps == false ? 1 : (int) Math.Floor(Math.Log(Math.Max(Width, Height), 2)));

            GlTexture = _gl.GenTexture();
            Bind();

#if GLES || LEGACY
            PixelFormat pxFormat = PixelFormat.Rgba;
#elif GL
            PixelFormat pxFormat = PixelFormat.Bgra;
#endif

            _gl.TexStorage2D(GLEnum.Texture2D, MipmapLevels, InternalFormat, Width, Height);
            _gl.TexSubImage2D(GLEnum.Texture2D, 0, 0, 0, Width, Height, pxFormat, PixelType.UnsignedByte, (void*) data);

            if (generateMipmaps)
#if GLES
                _gl.GenerateMipmap(GLEnum.Texture2D);
#elif GL
                _gl.GenerateTextureMipmap(GlTexture);
#endif
            SetWrap(TextureCoordinate.S, TextureWrapMode.Repeat);
            SetWrap(TextureCoordinate.T, TextureWrapMode.Repeat);
            var mipmapLevels = MipmapLevels - 1;
            _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMaxLevel, in mipmapLevels);
        }

        public void Bind()
        {
            _gl.BindTexture(GLEnum.Texture2D, GlTexture);
        }

        public void SetMinFilter(TextureMinFilter textureMinFilter)
        {
            var filter = (int)textureMinFilter;
            _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMinFilter, in filter);
        }

        public void SetMagFilter(TextureMagFilter textureMagFilter)
        {
            var filter = (int)textureMagFilter;
            _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMagFilter, in filter);
        }

        public void SetAnisotropy(float level)
        {
            const TextureParameterName textureMaxAnisotropy = (TextureParameterName) 0x84FE;
            _gl.TexParameter(GLEnum.Texture2D, (GLEnum) textureMaxAnisotropy, Util.Clamp(level, 1, MaxAniso.GetValueOrDefault()));
        }

        public void SetLod(int @base, int min, int max)
        {
            _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureLodBias, in @base);
            _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMinLod, in min);
            _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMaxLod, in max);
        }

        public void SetWrap(TextureCoordinate coord, TextureWrapMode textureWrapMode)
        {
            var mode = (int)textureWrapMode;
            _gl.TexParameterI(GLEnum.Texture2D, (TextureParameterName) coord, in mode);
        }

        public void Dispose()
        {
            _gl.DeleteTexture(GlTexture);
        }
    }
}
