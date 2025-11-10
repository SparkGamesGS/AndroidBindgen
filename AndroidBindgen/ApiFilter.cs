// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using Cecla;

namespace AndroidBindgen;

public interface IApiFilter
{
    static readonly IApiFilter FacadeFilter = new Facade();
    static readonly IApiFilter AllFilter = new All();
    bool ShouldInclude(FieldInfo field);
    bool ShouldInclude(MethodInfo method);
    bool ShouldIncludeProxy();

    #region Nested type: All

    private class All : IApiFilter
    {
        #region IApiFilter Members

        public bool ShouldInclude(FieldInfo field)
        {
            return true;
        }

        public bool ShouldInclude(MethodInfo method)
        {
            return true;
        }

        public bool ShouldIncludeProxy()
        {
            return true;
        }

        #endregion
    }

    #endregion

    #region Nested type: Facade

    private class Facade : IApiFilter
    {
        #region IApiFilter Members

        public bool ShouldInclude(FieldInfo field)
        {
            return false;
        }

        public bool ShouldInclude(MethodInfo method)
        {
            return false;
        }

        public bool ShouldIncludeProxy()
        {
            return false;
        }

        #endregion
    }

    #endregion
}
