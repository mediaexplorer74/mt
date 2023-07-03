// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamHTTP
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

namespace Steamworks
{
  public static class SteamHTTP
  {
    public static HTTPRequestHandle CreateHTTPRequest(
      EHTTPMethod eHTTPRequestMethod,
      string pchAbsoluteURL)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchAbsoluteURL1 = new InteropHelp.UTF8StringHandle(pchAbsoluteURL))
        return (HTTPRequestHandle) NativeMethods.ISteamHTTP_CreateHTTPRequest(eHTTPRequestMethod, pchAbsoluteURL1);
    }

    public static bool SetHTTPRequestContextValue(HTTPRequestHandle hRequest, ulong ulContextValue)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTTP_SetHTTPRequestContextValue(hRequest, ulContextValue);
    }

    public static bool SetHTTPRequestNetworkActivityTimeout(
      HTTPRequestHandle hRequest,
      uint unTimeoutSeconds)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTTP_SetHTTPRequestNetworkActivityTimeout(hRequest, unTimeoutSeconds);
    }

    public static bool SetHTTPRequestHeaderValue(
      HTTPRequestHandle hRequest,
      string pchHeaderName,
      string pchHeaderValue)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchHeaderName1 = new InteropHelp.UTF8StringHandle(pchHeaderName))
      {
        using (InteropHelp.UTF8StringHandle pchHeaderValue1 = new InteropHelp.UTF8StringHandle(pchHeaderValue))
          return NativeMethods.ISteamHTTP_SetHTTPRequestHeaderValue(hRequest, pchHeaderName1, pchHeaderValue1);
      }
    }

    public static bool SetHTTPRequestGetOrPostParameter(
      HTTPRequestHandle hRequest,
      string pchParamName,
      string pchParamValue)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchParamName1 = new InteropHelp.UTF8StringHandle(pchParamName))
      {
        using (InteropHelp.UTF8StringHandle pchParamValue1 = new InteropHelp.UTF8StringHandle(pchParamValue))
          return NativeMethods.ISteamHTTP_SetHTTPRequestGetOrPostParameter(hRequest, pchParamName1, pchParamValue1);
      }
    }

    public static bool SendHTTPRequest(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTTP_SendHTTPRequest(hRequest, out pCallHandle);
    }

    public static bool SendHTTPRequestAndStreamResponse(
      HTTPRequestHandle hRequest,
      out SteamAPICall_t pCallHandle)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTTP_SendHTTPRequestAndStreamResponse(hRequest, out pCallHandle);
    }

    public static bool DeferHTTPRequest(HTTPRequestHandle hRequest)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTTP_DeferHTTPRequest(hRequest);
    }

    public static bool PrioritizeHTTPRequest(HTTPRequestHandle hRequest)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTTP_PrioritizeHTTPRequest(hRequest);
    }

    public static bool GetHTTPResponseHeaderSize(
      HTTPRequestHandle hRequest,
      string pchHeaderName,
      out uint unResponseHeaderSize)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchHeaderName1 = new InteropHelp.UTF8StringHandle(pchHeaderName))
        return NativeMethods.ISteamHTTP_GetHTTPResponseHeaderSize(hRequest, pchHeaderName1, out unResponseHeaderSize);
    }

    public static bool GetHTTPResponseHeaderValue(
      HTTPRequestHandle hRequest,
      string pchHeaderName,
      byte[] pHeaderValueBuffer,
      uint unBufferSize)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchHeaderName1 = new InteropHelp.UTF8StringHandle(pchHeaderName))
        return NativeMethods.ISteamHTTP_GetHTTPResponseHeaderValue(hRequest, pchHeaderName1, pHeaderValueBuffer, unBufferSize);
    }

    public static bool GetHTTPResponseBodySize(HTTPRequestHandle hRequest, out uint unBodySize)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTTP_GetHTTPResponseBodySize(hRequest, out unBodySize);
    }

    public static bool GetHTTPResponseBodyData(
      HTTPRequestHandle hRequest,
      byte[] pBodyDataBuffer,
      uint unBufferSize)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTTP_GetHTTPResponseBodyData(hRequest, pBodyDataBuffer, unBufferSize);
    }

    public static bool GetHTTPStreamingResponseBodyData(
      HTTPRequestHandle hRequest,
      uint cOffset,
      byte[] pBodyDataBuffer,
      uint unBufferSize)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTTP_GetHTTPStreamingResponseBodyData(hRequest, cOffset, pBodyDataBuffer, unBufferSize);
    }

    public static bool ReleaseHTTPRequest(HTTPRequestHandle hRequest)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTTP_ReleaseHTTPRequest(hRequest);
    }

    public static bool GetHTTPDownloadProgressPct(
      HTTPRequestHandle hRequest,
      out float pflPercentOut)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTTP_GetHTTPDownloadProgressPct(hRequest, out pflPercentOut);
    }

    public static bool SetHTTPRequestRawPostBody(
      HTTPRequestHandle hRequest,
      string pchContentType,
      byte[] pubBody,
      uint unBodyLen)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchContentType1 = new InteropHelp.UTF8StringHandle(pchContentType))
        return NativeMethods.ISteamHTTP_SetHTTPRequestRawPostBody(hRequest, pchContentType1, pubBody, unBodyLen);
    }

    public static HTTPCookieContainerHandle CreateCookieContainer(bool bAllowResponsesToModify)
    {
      InteropHelp.TestIfAvailableClient();
      return (HTTPCookieContainerHandle) NativeMethods.ISteamHTTP_CreateCookieContainer(bAllowResponsesToModify);
    }

    public static bool ReleaseCookieContainer(HTTPCookieContainerHandle hCookieContainer)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTTP_ReleaseCookieContainer(hCookieContainer);
    }

    public static bool SetCookie(
      HTTPCookieContainerHandle hCookieContainer,
      string pchHost,
      string pchUrl,
      string pchCookie)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchHost1 = new InteropHelp.UTF8StringHandle(pchHost))
      {
        using (InteropHelp.UTF8StringHandle pchUrl1 = new InteropHelp.UTF8StringHandle(pchUrl))
        {
          using (InteropHelp.UTF8StringHandle pchCookie1 = new InteropHelp.UTF8StringHandle(pchCookie))
            return NativeMethods.ISteamHTTP_SetCookie(hCookieContainer, pchHost1, pchUrl1, pchCookie1);
        }
      }
    }

    public static bool SetHTTPRequestCookieContainer(
      HTTPRequestHandle hRequest,
      HTTPCookieContainerHandle hCookieContainer)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTTP_SetHTTPRequestCookieContainer(hRequest, hCookieContainer);
    }

    public static bool SetHTTPRequestUserAgentInfo(
      HTTPRequestHandle hRequest,
      string pchUserAgentInfo)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchUserAgentInfo1 = new InteropHelp.UTF8StringHandle(pchUserAgentInfo))
        return NativeMethods.ISteamHTTP_SetHTTPRequestUserAgentInfo(hRequest, pchUserAgentInfo1);
    }

    public static bool SetHTTPRequestRequiresVerifiedCertificate(
      HTTPRequestHandle hRequest,
      bool bRequireVerifiedCertificate)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTTP_SetHTTPRequestRequiresVerifiedCertificate(hRequest, bRequireVerifiedCertificate);
    }

    public static bool SetHTTPRequestAbsoluteTimeoutMS(
      HTTPRequestHandle hRequest,
      uint unMilliseconds)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTTP_SetHTTPRequestAbsoluteTimeoutMS(hRequest, unMilliseconds);
    }

    public static bool GetHTTPRequestWasTimedOut(HTTPRequestHandle hRequest, out bool pbWasTimedOut)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTTP_GetHTTPRequestWasTimedOut(hRequest, out pbWasTimedOut);
    }
  }
}
