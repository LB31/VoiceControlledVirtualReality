﻿/**
* Copyright 2015 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/

using FullSerializer;
using IBM.Watson.DeveloperCloud.Connection;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.Assistant.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleAssistantV1 : MonoBehaviour
{
    #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
    [Space(10)]
    [Tooltip("The service URL (optional). This defaults to \"https://gateway.watsonplatform.net/assistant/api\"")]
    [SerializeField]
    private string _serviceUrl;
    [Tooltip("The workspaceId to run the example.")]
    [SerializeField]
    private string _workspaceId;
    [Tooltip("The version date with which you would like to use the service in the form YYYY-MM-DD.")]
    [SerializeField]
    private string _versionDate;
    [Header("CF Authentication")]
    [Tooltip("The authentication username.")]
    [SerializeField]
    private string _username;
    [Tooltip("The authentication password.")]
    [SerializeField]
    private string _password;
    [Header("IAM Authentication")]
    [Tooltip("The IAM apikey.")]
    [SerializeField]
    private string _iamApikey;
    [Tooltip("The IAM url used to authenticate the apikey (optional). This defaults to \"https://iam.bluemix.net/identity/token\".")]
    [SerializeField]
    private string _iamUrl;
    #endregion

    private string _createdWorkspaceId;

    private Assistant _service;

    private fsSerializer _serializer = new fsSerializer();

    private string _inputString = "Hello";
    private string _conversationString0 = "unlock the door";
    private string _conversationString1 = "turn on the ac";
    private string _conversationString2 = "turn down the radio";

    private static string _createdWorkspaceName = "unity-sdk-example-workspace-delete";
    private static string _createdWorkspaceDescription = "A Workspace created by the Unity SDK Assistant example script. Please delete this.";
    private static string _createdWorkspaceLanguage = "en";
    private static string _createdEntity = "untiyEntity";
    private static string _createdEntityDescription = "Entity created by the Unity SDK Assistant example script.";
    private static string _createdValue = "untiyuntiyalue";
    private static string _createdIntent = "untiyIntent";
    private static string _createdIntentDescription = "Intent created by the Unity SDK Assistant example script.";
    private static string _createdCounterExampleText = "untiyExample text";
    private static string _createdSynonym = "untiySynonym";
    private static string _createdExample = "untiyExample";
    private static string _dialogNodeName = "untiyDialognode";
    private static string _dialogNodeDesc = "Unity SDK Integration test dialog node";
    private Dictionary<string, object> _context = null;

    private bool _listWorkspacesTested = false;
    private bool _createWorkspaceTested = false;
    private bool _getWorkspaceTested = false;
    private bool _updateWorkspaceTested = false;
    private bool _messageTested = false;
    private bool _listIntentsTested = false;
    private bool _createIntentTested = false;
    private bool _getIntentTested = false;
    private bool _updateIntentTested = false;
    private bool _listExamplesTested = false;
    private bool _createExampleTested = false;
    private bool _getExampleTested = false;
    private bool _updateExampleTested = false;
    private bool _listEntitiesTested = false;
    private bool _createEntityTested = false;
    private bool _getEntityTested = false;
    private bool _updateEntityTested = false;
    private bool _listMentionsTested = false;
    private bool _listValuesTested = false;
    private bool _createValueTested = false;
    private bool _getValueTested = false;
    private bool _updateValueTested = false;
    private bool _listSynonymsTested = false;
    private bool _createSynonymTested = false;
    private bool _getSynonymTested = false;
    private bool _updateSynonymTested = false;
    private bool _listDialogNodesTested = false;
    private bool _createDialogNodeTested = false;
    private bool _getDialogNodeTested = false;
    private bool _updateDialogNodeTested = false;
    private bool _listLogsInWorkspaceTested = false;
    private bool _listAllLogsTested = false;
    private bool _listCounterexamplesTested = false;
    private bool _createCounterexampleTested = false;
    private bool _getCounterexampleTested = false;
    private bool _updateCounterexampleTested = false;
    private bool _deleteCounterexampleTested = false;
    private bool _deleteDialogNodeTested = false;
    private bool _deleteSynonymTested = false;
    private bool _deleteValueTested = false;
    private bool _deleteEntityTested = false;
    private bool _deleteExampleTested = false;
    private bool _deleteIntentTested = false;
    private bool _deleteWorkspaceTested = false;

    void Start()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
    }

    private IEnumerator CreateService()
    {
        //  Create credential and instantiate service
        Credentials credentials = null;
        if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
        {
            //  Authenticate using username and password
            credentials = new Credentials(_username, _password, _serviceUrl);
        }
        else if (!string.IsNullOrEmpty(_iamApikey))
        {
            //  Authenticate using iamApikey
            TokenOptions tokenOptions = new TokenOptions()
            {
                IamApiKey = _iamApikey,
                IamUrl = _iamUrl
            };

            credentials = new Credentials(tokenOptions, _serviceUrl);

            //  Wait for tokendata
            while (!credentials.HasIamTokenData())
                yield return null;
        }
        else
        {
            throw new WatsonException("Please provide either username and password or IAM apikey to authenticate the service."); 
        }

        _service = new Assistant(credentials);
        _service.VersionDate = _versionDate;

        Runnable.Run(Examples());
    }

    private IEnumerator Examples()
    {
        //  List Workspaces
        _service.ListWorkspaces(OnListWorkspaces, OnFail);
        while (!_listWorkspacesTested)
            yield return null;
        //  Create Workspace
        CreateWorkspace workspace = new CreateWorkspace()
        {
            Name = _createdWorkspaceName,
            Description = _createdWorkspaceDescription,
            Language = _createdWorkspaceLanguage,
            LearningOptOut = true
        };
        _service.CreateWorkspace(OnCreateWorkspace, OnFail, workspace);
        while (!_createWorkspaceTested)
            yield return null;
        //  Get Workspace
        _service.GetWorkspace(OnGetWorkspace, OnFail, _createdWorkspaceId);
        while (!_getWorkspaceTested)
            yield return null;
        //  Update Workspace
        UpdateWorkspace updateWorkspace = new UpdateWorkspace()
        {
            Name = _createdWorkspaceName + "Updated",
            Description = _createdWorkspaceDescription + "Updated",
            Language = _createdWorkspaceLanguage
        };
        _service.UpdateWorkspace(OnUpdateWorkspace, OnFail, _createdWorkspaceId, updateWorkspace);
        while (!_updateWorkspaceTested)
            yield return null;

        //  Message
        Dictionary<string, object> input = new Dictionary<string, object>();
        input.Add("text", _inputString);
        MessageRequest messageRequest = new MessageRequest()
        {
            Input = input
        };
        _service.Message(OnMessage, OnFail, _workspaceId, messageRequest);
        while (!_messageTested)
            yield return null;
        _messageTested = false;

        input["text"] = _conversationString0;
        MessageRequest messageRequest0 = new MessageRequest()
        {
            Input = input,
            Context = _context
        };
        _service.Message(OnMessage, OnFail, _workspaceId, messageRequest0);
        while (!_messageTested)
            yield return null;
        _messageTested = false;

        input["text"] = _conversationString1;
        MessageRequest messageRequest1 = new MessageRequest()
        {
            Input = input,
            Context = _context
        };
        _service.Message(OnMessage, OnFail, _workspaceId, messageRequest1);
        while (!_messageTested)
            yield return null;
        _messageTested = false;

        input["text"] = _conversationString2;
        MessageRequest messageRequest2 = new MessageRequest()
        {
            Input = input,
            Context = _context
        };
        _service.Message(OnMessage, OnFail, _workspaceId, messageRequest2);
        while (!_messageTested)
            yield return null;

        //  List Intents
        _service.ListIntents(OnListIntents, OnFail, _createdWorkspaceId);
        while (!_listIntentsTested)
            yield return null;
        //  Create Intent
        CreateIntent createIntent = new CreateIntent()
        {
            Intent = _createdIntent,
            Description = _createdIntentDescription
        };
        _service.CreateIntent(OnCreateIntent, OnFail, _createdWorkspaceId, createIntent);
        while (!_createIntentTested)
            yield return null;
        //  Get Intent
        _service.GetIntent(OnGetIntent, OnFail, _createdWorkspaceId, _createdIntent);
        while (!_getIntentTested)
            yield return null;
        //  Update Intents
        string updatedIntent = _createdIntent + "Updated";
        string updatedIntentDescription = _createdIntentDescription + "Updated";
        UpdateIntent updateIntent = new UpdateIntent()
        {
            Intent = updatedIntent,
            Description = updatedIntentDescription
        };
        _service.UpdateIntent(OnUpdateIntent, OnFail, _createdWorkspaceId, _createdIntent, updateIntent);
        while (!_updateIntentTested)
            yield return null;

        //  List Examples
        _service.ListExamples(OnListExamples, OnFail, _createdWorkspaceId, updatedIntent);
        while (!_listExamplesTested)
            yield return null;
        //  Create Examples
        CreateExample createExample = new CreateExample()
        {
            Text = _createdExample
        };
        _service.CreateExample(OnCreateExample, OnFail, _createdWorkspaceId, updatedIntent, createExample);
        while (!_createExampleTested)
            yield return null;
        //  Get Example
        _service.GetExample(OnGetExample, OnFail, _createdWorkspaceId, updatedIntent, _createdExample);
        while (!_getExampleTested)
            yield return null;
        //  Update Examples
        string updatedExample = _createdExample + "Updated";
        UpdateExample updateExample = new UpdateExample()
        {
            Text = updatedExample
        };
        _service.UpdateExample(OnUpdateExample, OnFail, _createdWorkspaceId, updatedIntent, _createdExample, updateExample);
        while (!_updateExampleTested)
            yield return null;

        //  List Entities
        _service.ListEntities(OnListEntities, OnFail, _createdWorkspaceId);
        while (!_listEntitiesTested)
            yield return null;
        //  Create Entities
        CreateEntity entity = new CreateEntity()
        {
            Entity = _createdEntity,
            Description = _createdEntityDescription
        };
        _service.CreateEntity(OnCreateEntity, OnFail, _createdWorkspaceId, entity);
        while (!_createEntityTested)
            yield return null;
        //  Get Entity
        _service.GetEntity(OnGetEntity, OnFail, _createdWorkspaceId, _createdEntity);
        while (!_getEntityTested)
            yield return null;
        //  Update Entities
        string updatedEntity = _createdEntity + "Updated";
        string updatedEntityDescription = _createdEntityDescription + "Updated";
        UpdateEntity updateEntity = new UpdateEntity()
        {
            Entity = updatedEntity,
            Description = updatedEntityDescription
        };
        _service.UpdateEntity(OnUpdateEntity, OnFail, _createdWorkspaceId, _createdEntity, updateEntity);
        while (!_updateEntityTested)
            yield return null;

        // List Mentinos
        _service.ListMentions(OnListMentions, OnFail, _createdWorkspaceId, updatedEntity);
        while (!_listMentionsTested)
            yield return null;

        //  List Values
        _service.ListValues(OnListValues, OnFail, _createdWorkspaceId, updatedEntity);
        while (!_listValuesTested)
            yield return null;
        //  Create Values
        CreateValue value = new CreateValue()
        {
            Value = _createdValue
        };
        _service.CreateValue(OnCreateValue, OnFail, _createdWorkspaceId, updatedEntity, value);
        while (!_createValueTested)
            yield return null;
        //  Get Value
        _service.GetValue(OnGetValue, OnFail, _createdWorkspaceId, updatedEntity, _createdValue);
        while (!_getValueTested)
            yield return null;
        //  Update Values
        string updatedValue = _createdValue + "Updated";
        UpdateValue updateValue = new UpdateValue()
        {
            Value = updatedValue
        };
        _service.UpdateValue(OnUpdateValue, OnFail, _createdWorkspaceId, updatedEntity, _createdValue, updateValue);
        while (!_updateValueTested)
            yield return null;

        //  List Synonyms
        _service.ListSynonyms(OnListSynonyms, OnFail, _createdWorkspaceId, updatedEntity, updatedValue);
        while (!_listSynonymsTested)
            yield return null;
        //  Create Synonyms
        CreateSynonym synonym = new CreateSynonym()
        {
            Synonym = _createdSynonym
        };
        _service.CreateSynonym(OnCreateSynonym, OnFail, _createdWorkspaceId, updatedEntity, updatedValue, synonym);
        while (!_createSynonymTested)
            yield return null;
        //  Get Synonym
        _service.GetSynonym(OnGetSynonym, OnFail, _createdWorkspaceId, updatedEntity, updatedValue, _createdSynonym);
        while (!_getSynonymTested)
            yield return null;
        //  Update Synonyms
        string updatedSynonym = _createdSynonym + "Updated";
        UpdateSynonym updateSynonym = new UpdateSynonym()
        {
            Synonym = updatedSynonym
        };
        _service.UpdateSynonym(OnUpdateSynonym, OnFail, _createdWorkspaceId, updatedEntity, updatedValue, _createdSynonym, updateSynonym);
        while (!_updateSynonymTested)
            yield return null;

        //  List Dialog Nodes
        _service.ListDialogNodes(OnListDialogNodes, OnFail, _createdWorkspaceId);
        while (!_listDialogNodesTested)
            yield return null;
        //  Create Dialog Nodes
        CreateDialogNode createDialogNode = new CreateDialogNode()
        {
            DialogNode = _dialogNodeName,
            Description = _dialogNodeDesc
        };
        _service.CreateDialogNode(OnCreateDialogNode, OnFail, _createdWorkspaceId, createDialogNode);
        while (!_createDialogNodeTested)
            yield return null;
        //  Get Dialog Node
        _service.GetDialogNode(OnGetDialogNode, OnFail, _createdWorkspaceId, _dialogNodeName);
        while (!_getDialogNodeTested)
            yield return null;
        //  Update Dialog Nodes
        string updatedDialogNodeName = _dialogNodeName + "Updated";
        string updatedDialogNodeDescription = _dialogNodeDesc + "Updated";
        UpdateDialogNode updateDialogNode = new UpdateDialogNode()
        {
            DialogNode = updatedDialogNodeName,
            Description = updatedDialogNodeDescription
        };
        _service.UpdateDialogNode(OnUpdateDialogNode, OnFail, _createdWorkspaceId, _dialogNodeName, updateDialogNode);
        while (!_updateDialogNodeTested)
            yield return null;

        //  List Logs In Workspace
        _service.ListLogs(OnListLogs, OnFail, _createdWorkspaceId);
        while (!_listLogsInWorkspaceTested)
            yield return null;
        //  List All Logs
        var filter = "(language::en,request.context.metadata.deployment::deployment_1)";
        _service.ListAllLogs(OnListAllLogs, OnFail, filter);
        while (!_listAllLogsTested)
            yield return null;

        //  List Counterexamples
        _service.ListCounterexamples(OnListCounterexamples, OnFail, _createdWorkspaceId);
        while (!_listCounterexamplesTested)
            yield return null;
        //  Create Counterexamples
        CreateCounterexample example = new CreateCounterexample()
        {
            Text = _createdCounterExampleText
        };
        _service.CreateCounterexample(OnCreateCounterexample, OnFail, _createdWorkspaceId, example);
        while (!_createCounterexampleTested)
            yield return null;
        //  Get Counterexample
        _service.GetCounterexample(OnGetCounterexample, OnFail, _createdWorkspaceId, _createdCounterExampleText);
        while (!_getCounterexampleTested)
            yield return null;
        //  Update Counterexamples
        string updatedCounterExampleText = _createdCounterExampleText + "Updated";
        UpdateCounterexample updateCounterExample = new UpdateCounterexample()
        {
            Text = updatedCounterExampleText
        };
        _service.UpdateCounterexample(OnUpdateCounterexample, OnFail, _createdWorkspaceId, _createdCounterExampleText, updateCounterExample);
        while (!_updateCounterexampleTested)
            yield return null;

        //  Delete Counterexample
        _service.DeleteCounterexample(OnDeleteCounterexample, OnFail, _createdWorkspaceId, updatedCounterExampleText);
        while (!_deleteCounterexampleTested)
            yield return null;
        //  Delete Dialog Node
        _service.DeleteDialogNode(OnDeleteDialogNode, OnFail, _createdWorkspaceId, updatedDialogNodeName);
        while (!_deleteDialogNodeTested)
            yield return null;
        //  Delete Synonym
        _service.DeleteSynonym(OnDeleteSynonym, OnFail, _createdWorkspaceId, updatedEntity, updatedValue, updatedSynonym);
        while (!_deleteSynonymTested)
            yield return null;
        //  Delete Value
        _service.DeleteValue(OnDeleteValue, OnFail, _createdWorkspaceId, updatedEntity, updatedValue);
        while (!_deleteValueTested)
            yield return null;
        //  Delete Entity
        _service.DeleteEntity(OnDeleteEntity, OnFail, _createdWorkspaceId, updatedEntity);
        while (!_deleteEntityTested)
            yield return null;
        //  Delete Example
        _service.DeleteExample(OnDeleteExample, OnFail, _createdWorkspaceId, updatedIntent, updatedExample);
        while (!_deleteExampleTested)
            yield return null;
        //  Delete Intent
        _service.DeleteIntent(OnDeleteIntent, OnFail, _createdWorkspaceId, updatedIntent);
        while (!_deleteIntentTested)
            yield return null;
        //  Delete Workspace
        _service.DeleteWorkspace(OnDeleteWorkspace, OnFail, _createdWorkspaceId);
        while (!_deleteWorkspaceTested)
            yield return null;

        Log.Debug("ExampleAssistantV1.RunTest()", "Assistant examples complete.");

        yield break;
    }

    private void OnListMentions(EntityMentionCollection response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnListMentions()", "Response: {0}", customData["json"].ToString());
        _listMentionsTested = true;
    }

    private void OnDeleteWorkspace(object response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnDeleteWorkspace()", "Response: {0}", customData["json"].ToString());
        _deleteWorkspaceTested = true;
    }

    private void OnDeleteIntent(object response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnDeleteIntent()", "Response: {0}", customData["json"].ToString());
        _deleteIntentTested = true;
    }

    private void OnDeleteExample(object response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnDeleteExample()", "Response: {0}", customData["json"].ToString());
        _deleteExampleTested = true;
    }

    private void OnDeleteEntity(object response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnDeleteEntity()", "Response: {0}", customData["json"].ToString());
        _deleteEntityTested = true;
    }

    private void OnDeleteValue(object response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnDeleteValue()", "Response: {0}", customData["json"].ToString());
        _deleteValueTested = true;
    }

    private void OnDeleteSynonym(object response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnDeleteSynonym()", "Response: {0}", customData["json"].ToString());
        _deleteSynonymTested = true;
    }

    private void OnDeleteDialogNode(object response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnDeleteDialogNode()", "Response: {0}", customData["json"].ToString());
        _deleteDialogNodeTested = true;
    }

    private void OnDeleteCounterexample(object response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnDeleteCounterexample()", "Response: {0}", customData["json"].ToString());
        _deleteCounterexampleTested = true;
    }

    private void OnUpdateCounterexample(Counterexample response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnUpdateCounterexample()", "Response: {0}", customData["json"].ToString());
        _updateCounterexampleTested = true;
    }

    private void OnGetCounterexample(Counterexample response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnGetCounterexample()", "Response: {0}", customData["json"].ToString());
        _getCounterexampleTested = true;
    }

    private void OnCreateCounterexample(Counterexample response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnCreateCounterexample()", "Response: {0}", customData["json"].ToString());
        _createCounterexampleTested = true;
    }

    private void OnListCounterexamples(CounterexampleCollection response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnListCounterexamples()", "Response: {0}", customData["json"].ToString());
        _listCounterexamplesTested = true;
    }

    private void OnListAllLogs(LogCollection response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnListAllLogs()", "Response: {0}", customData["json"].ToString());
        _listAllLogsTested = true;
    }

    private void OnListLogs(LogCollection response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnListLogs()", "Response: {0}", customData["json"].ToString());
        _listLogsInWorkspaceTested = true;
    }

    private void OnUpdateDialogNode(DialogNode response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnUpdateDialogNode()", "Response: {0}", customData["json"].ToString());
        _updateDialogNodeTested = true;
    }

    private void OnGetDialogNode(DialogNode response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnGetDialogNode()", "Response: {0}", customData["json"].ToString());
        _getDialogNodeTested = true;
    }

    private void OnCreateDialogNode(DialogNode response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnCreateDialogNode()", "Response: {0}", customData["json"].ToString());
        _createDialogNodeTested = true;
    }

    private void OnListDialogNodes(DialogNodeCollection response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnListDialogNodes()", "Response: {0}", customData["json"].ToString());
        _listDialogNodesTested = true;
    }

    private void OnUpdateSynonym(Synonym response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnUpdateSynonym()", "Response: {0}", customData["json"].ToString());
        _updateSynonymTested = true;
    }

    private void OnGetSynonym(Synonym response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnGetSynonym()", "Response: {0}", customData["json"].ToString());
        _getSynonymTested = true;
    }

    private void OnCreateSynonym(Synonym response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnCreateSynonym()", "Response: {0}", customData["json"].ToString());
        _createSynonymTested = true;
    }

    private void OnListSynonyms(SynonymCollection response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnListSynonyms()", "Response: {0}", customData["json"].ToString());
        _listSynonymsTested = true;
    }

    private void OnUpdateValue(Value response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnUpdateValue()", "Response: {0}", customData["json"].ToString());
        _updateValueTested = true;
    }

    private void OnGetValue(ValueExport response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnGetValue()", "Response: {0}", customData["json"].ToString());
        _getValueTested = true;
    }

    private void OnCreateValue(Value response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnCreateValue()", "Response: {0}", customData["json"].ToString());
        _createValueTested = true;
    }

    private void OnListValues(ValueCollection response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnListValues()", "Response: {0}", customData["json"].ToString());
        _listValuesTested = true;
    }

    private void OnUpdateEntity(Entity response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnUpdateEntity()", "Response: {0}", customData["json"].ToString());
        _updateEntityTested = true;
    }

    private void OnGetEntity(EntityExport response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnGetEntity()", "Response: {0}", customData["json"].ToString());
        _getEntityTested = true;
    }

    private void OnCreateEntity(Entity response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnCreateEntity()", "Response: {0}", customData["json"].ToString());
        _createEntityTested = true;
    }

    private void OnListEntities(EntityCollection response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnListEntities()", "Response: {0}", customData["json"].ToString());
        _listEntitiesTested = true;
    }

    private void OnUpdateExample(Example response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnUpdateExample()", "Response: {0}", customData["json"].ToString());
        _updateExampleTested = true;
    }

    private void OnGetExample(Example response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnGetExample()", "Response: {0}", customData["json"].ToString());
        _getExampleTested = true;
    }

    private void OnCreateExample(Example response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnCreateExample()", "Response: {0}", customData["json"].ToString());
        _createExampleTested = true;
    }

    private void OnListExamples(ExampleCollection response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnListExamples()", "Response: {0}", customData["json"].ToString());
        _listExamplesTested = true;
    }

    private void OnUpdateIntent(Intent response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnUpdateIntent()", "Response: {0}", customData["json"].ToString());
        _updateIntentTested = true;
    }

    private void OnGetIntent(IntentExport response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnGetIntent()", "Response: {0}", customData["json"].ToString());
        _getIntentTested = true;
    }

    private void OnCreateIntent(Intent response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnCreateIntent()", "Response: {0}", customData["json"].ToString());
        _createIntentTested = true;
    }

    private void OnListIntents(IntentCollection response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnListIntents()", "Response: {0}", customData["json"].ToString());
        _listIntentsTested = true;
    }

    private void OnMessage(object response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnMessage()", "Response: {0}", customData["json"].ToString());

        //  Convert resp to fsdata
        fsData fsdata = null;
        fsResult r = _serializer.TrySerialize(response.GetType(), response, out fsdata);
        if (!r.Succeeded)
            throw new WatsonException(r.FormattedMessages);

        //  Convert fsdata to MessageResponse
        MessageResponse messageResponse = new MessageResponse();
        object obj = messageResponse;
        r = _serializer.TryDeserialize(fsdata, obj.GetType(), ref obj);
        if (!r.Succeeded)
            throw new WatsonException(r.FormattedMessages);

        //  Set context for next round of messaging
        object _tempContext = null;
        (response as Dictionary<string, object>).TryGetValue("context", out _tempContext);

        if (_tempContext != null)
            _context = _tempContext as Dictionary<string, object>;
        else
            Log.Debug("ExampleAssistantV1.OnMessage()", "Failed to get context");

        //  Get intent
        object tempIntentsObj = null;
        (response as Dictionary<string, object>).TryGetValue("intents", out tempIntentsObj);
        object tempIntentObj = (tempIntentsObj as List<object>)[0];
        object tempIntent = null;
        (tempIntentObj as Dictionary<string, object>).TryGetValue("intent", out tempIntent);
        string intent = tempIntent.ToString();

        Log.Debug("ExampleAssistantV1.OnMessage()", "intent: {0}", intent);

        _messageTested = true;
    }

    private void OnUpdateWorkspace(Workspace response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnUpdateWorkspace()", "Response: {0}", customData["json"].ToString());
        _updateWorkspaceTested = true;
    }

    private void OnGetWorkspace(WorkspaceExport response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnGetWorkspace()", "Response: {0}", customData["json"].ToString());
        _getWorkspaceTested = true;
    }

    private void OnCreateWorkspace(Workspace response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnCreateWorkspace()", "Response: {0}", customData["json"].ToString());
        _createdWorkspaceId = response.WorkspaceId;
        _createWorkspaceTested = true;
    }

    private void OnListWorkspaces(WorkspaceCollection response, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnListWorkspaces()", "Response: {0}", customData["json"].ToString());

        foreach(Workspace workspace in response.Workspaces)
        {
            if(workspace.Name.Contains("unity"))
                _service.DeleteWorkspace(OnDeleteWorkspace, OnFail, workspace.WorkspaceId);
        }

        _listWorkspacesTested = true;
    }

    private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistantV1.OnFail()", "Response: {0}", customData["json"].ToString());
        Log.Error("ExampleAssistantV1.OnFail()", "Error received: {0}", error.ToString());
    }
}
