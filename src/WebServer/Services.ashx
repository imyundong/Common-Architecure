<%@ WebHandler Language="VB" Class="Services" %>

Imports System
Imports System.Web
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow
Imports Workflow.Distributor

Public Class Services : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        CLog.Debug("User Host", context.Request.UserHostName)
        CLog.Debug("User Address", context.Request.UserHostAddress)
        CLog.Debug("Request Received")
        
        Threading.Thread.Sleep(900)
        Dim WorkflowDistributor As New CWorkflowDistributor
                
        Try
            Dim WorkflowData As CWorkflowData = WorkflowDistributor.Process(context.Request.InputStream, _
                                                                            context.Request.ContentEncoding)
            CLog.Info("Serilize Response")
            Dim FlowString As String = WorkflowData.ToString
            
            Dim Encoding As Text.Encoding = context.Request.ContentEncoding          
            ' Format to User Encoding
            Dim Buffer() As Byte = Encoding.GetBytes(FlowString)
            CLog.Sys("Send Response in {1} : {{{0}}}", Buffer.Length, Encoding.ToString)
            CLog.Dump(Buffer)
            
            context.Response.ContentType = "text/plain"
            context.Response.Write(FlowString)           
            
        Catch ex As Exception
            context.Response.StatusCode = 500
            context.Response.StatusDescription = ex.Message
        End Try
        
        'context.Response()
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class