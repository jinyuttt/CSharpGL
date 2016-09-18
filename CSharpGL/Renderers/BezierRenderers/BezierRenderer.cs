﻿using System.Collections.Generic;

namespace CSharpGL
{
    /// <summary>
    /// Rendering a 1D evaluator(a bezier curve) and its control points.
    /// </summary>
    public partial class Bezier1DRenderer : PointsRenderer
    {
        /// <summary>
        /// Creates a renderer that renders a 1D evaluator(a bezier curve) and its control points.
        /// </summary>
        /// <param name="controlPoints"></param>
        /// <returns></returns>
        public static Bezier1DRenderer Create(IList<vec3> controlPoints, BezierType type)
        {
            var shaderCodes = new ShaderCode[2];
            shaderCodes[0] = new ShaderCode(ManifestResourceLoader.LoadTextFile(@"Resources\Points.vert"), ShaderType.VertexShader);
            shaderCodes[1] = new ShaderCode(ManifestResourceLoader.LoadTextFile(@"Resources\Points.frag"), ShaderType.FragmentShader);
            var map = new CSharpGL.PropertyNameMap();
            map.Add("in_Position", Points.strposition);
            var model = new Points(controlPoints);
            var renderer = new Bezier1DRenderer(controlPoints, type, model, shaderCodes, map, Points.strposition);
            renderer.Lengths = model.Lengths;
            renderer.WorldPosition = model.WorldPosition;
            renderer.switchList.Add(new PointSizeSwitch(10));

            return renderer;
        }

        /// <summary>
        /// Rendering a 1D evaluator(a bezier curve) and its control points.
        /// </summary>
        /// <param name="controlPoints"></param>
        /// <param name="bufferable"></param>
        /// <param name="shaderCodes"></param>
        /// <param name="propertyNameMap"></param>
        /// <param name="positionNameInIBufferable"></param>
        /// <param name="switches"></param>
        private Bezier1DRenderer(IList<vec3> controlPoints, BezierType type, Points bufferable, CSharpGL.ShaderCode[] shaderCodes, CSharpGL.PropertyNameMap propertyNameMap, string positionNameInIBufferable, params GLSwitch[] switches) :
            base(bufferable, shaderCodes, propertyNameMap, positionNameInIBufferable, switches)
        {
            switch (type)
            {
                case BezierType.Curve:
                    this.Evaluator = new Evaluator1DRenderer(controlPoints);//, lengths);
                    break;
                case BezierType.Surface:
                    this.Evaluator = new Evaluator2DRenderer(controlPoints);//, lengths);
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// Rendering a 1D evaluator(a bezier curve).
        /// </summary>
        public EvaluatorRenderer Evaluator { get; private set; }

        /// <summary>
        ///
        /// </summary>
        protected override void DoInitialize()
        {
            base.DoInitialize();

            this.Evaluator.Initialize();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="arg"></param>
        protected override void DoRender(RenderEventArgs arg)
        {
            this.Evaluator.Render(arg);

            base.DoRender(arg);
        }

    }
}