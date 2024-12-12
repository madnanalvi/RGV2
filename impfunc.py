from sklearn.metrics import accuracy_score
import pandas as pd
import numpy as np

class infer():

    def detectMaliciousProcess(verbose,csvFile):
        func.println(verbose,'processing python script')
        logs=[]
        logs.append(pd.read_csv(csvFile, on_bad_lines='skip', low_memory=False, names=['TimeStamp',
            'TimeStampRelativeMSec',
            'ProcessID',
            'ParentProcessID',
            'ProcessName',
            'ThreadID',
            'ParentThreadID',
            'EventName',
            'EventID',
            'FilePath',
            'FileName',
            'FileType',
            'IrpPtr',
            'FileObject',
            'FileKey',
            'ExtraInfo',
            'InfoClass']))
        log = pd.concat(logs)
        #------------------------------------------------------------------------------------
        preProcessedData = func.preProcessData(log,verbose)
        if (len(preProcessedData.index) <= 2):
            func.println('1','||Rows:0')
            return 0
        df_consolidated = func.consolidate_Events1(preProcessedData,verbose)
        if (len(df_consolidated.index) <= 2):
            func.println('1','||Rows:0')
            return 0
        preds = df_consolidated['pid'].value_counts().max()
        pid = df_consolidated.groupby(['pid'])['pid'].count().idxmax()
        pn = df_consolidated.groupby(['pn'])['pn'].count().idxmax()
        total = df_consolidated.pid.count()
        maxper = (preds/total)*100
        msg= '||maxper='+ str(round(float(maxper),2))+' pn='+pn+' pid='+str(pid)+' count='+ str(int(preds))+' total='+ str(total)+' nan=0 all='+ str(df_consolidated.shape[0]);
        func.println('1',msg)
 
    def infer_tfidf(verbose,csvFile,modelpath,vocabpath,nfeatures,min,max): #entry
        func.println(verbose,'processing python script')
        logs=[]
        logs.append(pd.read_csv(csvFile, on_bad_lines='skip', low_memory=False, names=['TimeStamp',
            'TimeStampRelativeMSec',
            'ProcessID',
            'ParentProcessID',
            'ProcessName',
            'ThreadID',
            'ParentThreadID',
            'EventName',
            'EventID',
            'FilePath',
            'FileName',
            'FileType',
            'IrpPtr',
            'FileObject',
            'FileKey',
            'ExtraInfo',
            'InfoClass']))
        log = pd.concat(logs)
        #------------------------------------------------------------------------------------
        preProcessedData = func.preProcessData(log,verbose)
        consolidateData = func.consolidate_Events1(preProcessedData,verbose)
        if (len(consolidateData.index) <= 2):
            func.println('1','||Rows:0')
            return 0
        try:
            norm_features = infer.normalizeData_tfidf_infer(consolidateData,min,max,vocabpath,nfeatures,verbose)            
            func.println(verbose, 'normalize done')            
        except Exception as ex:
            import sys
            func.println('1', 'Error while Normalizing data.')
            exception_type, exception_object, exception_traceback = sys.exc_info()
            filename = exception_traceback.tb_frame.f_code.co_filename
            line_number = exception_traceback.tb_lineno
            func.println('1',"Exception type: "+ str(exception_type))
            func.println('1',"File name: "+ filename)
            func.println('1',"Line number: "+ str(line_number))            
            return 0   
        try:
            norm_features['pn'] = norm_features['pn'].astype(float)
            func.println(verbose,norm_features.shape)
        except Exception as ex:
            func.println('1', 'Type Error while casting.')
            func.println('1',ex)
            return 0
        #----------------------------------------------------------------------------------------
        try:
            from keras.models import load_model
            model = load_model(modelpath)
            #perd = model.signatures["serving_default"](**norm_features)
            #y_perd_dt = (perd['output_1'].numpy() > 0.5).astype("int32")
            y_perd = model.predict(norm_features)
            y_perd = (y_perd > 0.2).astype("int32")  
            consolidateData['y_perd_dt']=pd.Series(np.array(y_perd).flatten())          
            func.println(verbose, 'prediction success. Count: ' + str(len(y_perd)))
        except Exception as ex:
            func.println('1', 'error while prediction')
            func.println('1',ex)
            return 0
        #-------------------------------------------------------------------------------------
        preds = consolidateData[['fn','pn','pid','y_perd_dt']]
        nans = preds['y_perd_dt'].isna().sum()
        #predictions['y_perd_dt'] = predictions['y_perd_dt'].fillna(0)
        
        predictions = preds.dropna(axis=0)
        # generate output String
        msg=''
        try:
             if(len(predictions.index)<5):
                func.println('1','Too few Events Captured : plz wait.')
                msg='||maxper=0 pn=0 pid=0 count=0 total=0 nan=0 all=0'
             else:
                #func.println(verbose,str(predictions.groupby(['pid','pn']).sum()))
                mpid = predictions.groupby(['pid'])['y_perd_dt'].count().idxmax()
                total = predictions[(predictions['pid']==mpid)].y_perd_dt.count()
                pn = predictions.groupby(['pn'])['y_perd_dt'].count().idxmax()
                res = predictions[predictions['y_perd_dt'] == 1].groupby(['pid'])['y_perd_dt'].count()
                maxper = (res.max()/total)*100
                resmax=res.max()
                import math
                if math.isnan(maxper):maxper = 0
                if math.isnan(resmax):resmax = 0
                func.println(verbose,'Process : '+pn+'('+ str(mpid) + ') is malicious. Infection rate is. '+ str(maxper)+'%')
                msg= '||maxper='+ str(round(float(maxper),2))+' pn='+pn+' pid='+str(mpid)+' count='+ str(int(resmax))+' total='+ str(total)+' nan='+ str(nans)+' all='+ str(predictions.shape[0]);
        except Exception as e:
                func.println('1','output parse failed')
                func.println('1',e)
        #--------------------------------------------------------------------------------------
        func.println('1',msg)    
        
    def normalizeData_tfidf_infer(df1,min,max,vocabpath,nfeatures,verbose):  #main Method for normalize

        #--------------------------------------------tfidf--------------------------------------------------------------
        func.println(verbose,'120')
        func.println(verbose,'Tfidf--Vectorizer')  
        try:
            from sklearn.feature_extraction.text import TfidfTransformer,TfidfVectorizer
            import pickle
            loaded_vec = TfidfVectorizer(analyzer = 'char_wb', ngram_range = (min,max), decode_error="replace",vocabulary=pickle.load(open(vocabpath,"rb")))
            tfidf = TfidfTransformer().fit_transform(loaded_vec.fit_transform(df1['eid']))
            arr = tfidf.toarray()
            tfidf_tokens =loaded_vec.get_feature_names_out()
            #df_tfidf = pd.DataFrame(arr,columns=tfidf_tokens)
            df_tfidf = pd.DataFrame(arr)
            df_tfidf.columns = df_tfidf.columns.astype(str)
            df_tfidf = df_tfidf.add_prefix('col_')
            func.println(verbose,df_tfidf.columns.values.tolist())
            func.println(verbose,'145')
        except Exception as e:
            func.println('1',e)
            func.println('1','error in Tfidf_infer function')
            import sys
            exception_type, exception_object, exception_traceback = sys.exc_info()
            filename = exception_traceback.tb_frame.f_code.co_filename
            line_number = exception_traceback.tb_lineno
            func.println('1',"Line number: "+ str(line_number)) 
            return 0    
        #df_tfidf = functions.Tfidf_infer(vocabpath,df1['eid'],min,max,verbose)
        #----------------------------------------------------------------------------------------------------------------
        func.println(verbose,'122')
        df_tfidf['ts_ft_diff']=df1['ts_ft_diff']
        df_tfidf['ts_diff']=df1['ts_diff']
        df_tfidf['n_eid'] =df1['eid'].str.len()        
        func.println(verbose,'131')
        df_tfidf['ft']=df1['ft']
        df_tfidf['fn_ftc']=df1['fn_ftc']
        df_tfidf['pid_ftc']=df1['pid_ftc']
        df_tfidf['fts']=df1['fts']
        df_tfidf['pid']=df1['pid']
        df_tfidf['ppid']=df1['ppid']
        func.println(verbose,'121')
        df_tfidf = df_tfidf.fillna(0)
        #----------------------------------------------------LabelEncoder------------------------------------------------------------
        from sklearn.preprocessing import LabelEncoder
        le = LabelEncoder()
        df_tfidf['pn']=le.fit_transform(df1['pn'])
        df_tfidf['fn']= le.fit_transform(df1['fn'])
        #df_tfidf['pn']=functions.labelEncoder(df1,'pn')
        #---------------------------------------------------featuresSelection-------------------------------------------------------------
        try:
            func.println(verbose,'featuresSelection')           
            func.println(verbose,df_tfidf.columns.values.tolist())
            features = df_tfidf.loc[:, nfeatures]
        except Exception as e:
            func.println('1','error selecting features')
            func.println('1',e)
            return 0
        #---------------------------------------------------standardize-------------------------------------------------------------
        func.println(verbose,'standardizeStart')    
        #features = functions.standardizeUsingStandardScaler(features,verbose)
        try:
                from sklearn.preprocessing import StandardScaler
                scaler = StandardScaler()
                func.println(verbose,'233')    
                features = pd.DataFrame(scaler.fit_transform(features.values), columns = features.columns)
        except:
                func.println('1','error during standadscaler')
                return 0
        #----------------------------------------------------------------------------------------------------------------
        func.println(verbose,'normalizing completed')
        return features

class func():
        
    def preProcessData(log,verbose):  #in uuse for preprocess
      #Remove Nulls
      df = log.replace(np.inf, np.nan)
      df = df.dropna(axis=0)
      df = df.loc[:, ['FilePath','FileName','FileType','EventName','EventID','ProcessID','ParentProcessID','ProcessName','TimeStampRelativeMSec']]
      #timestamp diff
      td=df.groupby(['FileName', 'ProcessID'], group_keys=False)['TimeStampRelativeMSec'].min()
      df = pd.merge(df, td,how='right', on=['ProcessID','FileName'])
      df.rename(columns = {'TimeStampRelativeMSec_y':'ts'}, inplace = True)
      df=df.drop('TimeStampRelativeMSec_x',axis=1)
      df.drop_duplicates(inplace=True)
      #func.println(verbose,df.shape)
      #Join FileName and FilePath columns
      df['FileName'] = df['FilePath']+'\\'+ df['FileName']
      #Remove FileRundown events to make fileobject primary key
      #df = log.loc[(log['EventName']!='FileIO/FileRundown')]
      #Encode FileType column
      df['FType'] = df['FileType'].apply(lambda x : func.encode(str(x)[0:7]))
      #add column fts i.e exe+wancry+wancrpyt
      ftsum = df.groupby(['FileName','ProcessID'])['FType'].sum().reset_index(name='fts')
      df = pd.merge(df, ftsum,how='right', on=['FileName','ProcessID'])
      #Add EventID Column from EventName
      df['EventID'] = df['EventName'].apply(func.get_event_id)
      #Agregate Events Column
      events = df.groupby(['FileName','ProcessID'], as_index=False).agg({'EventID' : ''.join})
      merge_events = pd.merge(df, events,how='right', on=['FileName','ProcessID'])
      aggregated = merge_events.loc[:, ['FileName','FType','FileType','EventID_y','ProcessID','ParentProcessID','fts','ProcessName','ts']]
      #rename Columns
      aggregated.rename(columns = {'FileName':'fname','FType':'ft','EventID_y':'eid','ProcessID':'pid','ParentProcessID':'ppid','ProcessName':'pn'}, inplace = True)
      aggregated.drop_duplicates(inplace=True)
      #func.println(verbose,'Aggregated: '+ str(aggregated.shape))
      #Add coulum ftype_count (File Types Count wrt pid )
      FileType_count = aggregated.groupby(['pid'])['ft'].size().reset_index(name='pid_ftc')
      aggregated = pd.merge(aggregated, FileType_count,how='right', on=['pid']).reset_index()
      #Add coulum ftype_count (File Types Count wrt fn )
      FileType_count = aggregated.groupby(['fname'])['ft'].size().reset_index(name='fn_ftc')
      aggregated = pd.merge(aggregated, FileType_count,how='right', on=['fname']).reset_index()
      #encode eid
      #import sys
      #sys.set_int_max_str_digits(10000)
      #aggregated['eid'] = aggregated.eid.apply(lambda x: int(x, 36))
      df_td=aggregated.loc[:, ['ts','fname','fn_ftc','pid_ftc','fts','ft','FileType','eid','pn','pid','ppid']]
      df_td.reset_index()
      #func.println(verbose,df_td.columns)
      #func.println(verbose,df_td.shape)
      return df_td
    
    def consolidate_Events1(df_Processed,verbose):  #main Method for normalize

        df1 = df_Processed[ ( (df_Processed.eid.str.contains('a')) &(df_Processed.eid.str.contains('c'))&(df_Processed.eid.str.contains('d')))|
                    ( (df_Processed.eid.str.contains('b')) &(df_Processed.eid.str.contains('c'))&(df_Processed.eid.str.contains('d')))]
        func.println(verbose,df1.shape)

        #reset index --important--
        df1=df1.reset_index()
        #----------------------------------------------------------------------------------------------------------------
        func.println(verbose,'122')
        df1['ts'] = df1['ts'].astype(float)
        df1['pid'] = df1['pid'].astype(int)
        #Sort dataframe wrt ts,pid
        df=df1.sort_values(['ts','pid','pn'])
        #add time diff column i.e time between multiple accees to same [File Type] by a process
        df1['ts_ft_diff'] = df.groupby(['pid','ft','pn'])['ts'].diff()
        #add time diff column i.e time between multiple accees to [Files] by a process
        df1['ts_diff'] = df.groupby(['pid','pn'])['ts'].diff()
        #----------------------------------------------------------------------------------------------------------------
        df1.drop('ts',axis=1,inplace=True)
        df1.drop('index',axis=1,inplace=True)
        return df1
    
    def consolidate_Events(df1,verbose):  #main Method for normalize

        #remove rows with null pid
        df1 = df1[~df1.pid.isin(df1[df1['pid']==-1].index)]
        func.println(verbose,df1.shape)

        #remove rows without read or write events
        func.println(verbose,'122')
        indices_toremove = df1[df1['eid'].str.contains('c|d')==False].index
        df1 = df1.drop(indices_toremove)
        func.println(verbose,df1.shape)
        
        #remove rows withevents < =3
        func.println(verbose,'115')
        df1=df1.assign(eid_g4 = np.where(((df1['eid'].str.len()>=3)==True), '1', '0'))
        indices_toremove = df1[(df1['eid_g4']=='0')].index
        df1.drop(indices_toremove, inplace=True)
        df1.drop('eid_g4',axis=1, inplace=True)
        func.println(verbose,df1.shape)

        #remove rows with very rare event patterns <= 5
        value_counts = df1.eid.value_counts()
        to_remove = value_counts[value_counts <= 5].index
        df1 = df1[~df1.eid.isin(to_remove)]

        #reset index --important--
        df1=df1.reset_index()
        #----------------------------------------------------------------------------------------------------------------
        func.println(verbose,'122')
        df1['ts'] = df1['ts'].astype(float)
        df1['pid'] = df1['pid'].astype(int)
        #Sort dataframe wrt ts,pid
        df=df1.sort_values(['ts','pid','pn'])
        #add time diff column i.e time between multiple accees to same [File Type] by a process
        df1['ts_ft_diff'] = df.groupby(['pid','ft','pn'])['ts'].diff()
        #add time diff column i.e time between multiple accees to [Files] by a process
        df1['ts_diff'] = df.groupby(['pid','pn'])['ts'].diff()        
        #----------------------------------------------------------------------------------------------------------------
        df1.drop('ts',axis=1,inplace=True)
        df1.drop('index',axis=1,inplace=True)
        return df1 
        
    def encode(str1): 
      import binascii
      return int(binascii.hexlify(str1.encode("utf-8")), 16)    
    
    def decode(int_str): 
      import binascii
      return binascii.unhexlify(format(int_str, "x").encode("utf-8")).decode("utf-8")    
    
    def get_event_id(event_name):
        event_id_mapping = {
            "FileIO/Rename": "a",
            "FileIO/Delete": "b",
            "FileIO/Read": "c",
            "FileIO/Write": "d",
            "FileIO/SetInfo": "",
            "FileIO/Create": "f",
            "FileIO/Close": "g",
            "FileIO/FileCreate": "",
            "FileIO/FileDelete": "",
            "FileIO/FSControl": "",
            "FileIO/DirEnum": "",
            "FileIO/Cleanup": "l",
            "FileIO/DirNotify": "",
            "FileIO/QueryInfo": "n",
            "FileIO/Flush": "",
            "FileIO/OperationEnd": "",
            "FileIO/FileRundown": "",
            "Thread/DCStart": "",
            "Thread/Start": "",
            "Thread/Stop": "",
            "Thread/SetName": "",
            "Process/DCStart": "",
            "Process/Start": "",
            "Process/Stop": "",
            "EventTrace/RundownComplete": "",
            "EventTrace/Extension": "",
            "EventTrace/EndExtension": "",
            "Registry/SetValue": "",
            "Registry/SetInformation": "",
            "Registry/EnumerateKey": "",
            "Registry/EnumerateValueKey": "",
            "Registry/Open": "",
            "Registry/Close": "",
            "Registry/QueryValue": "",
            "Registry/QueryMultipleValue": "",
            "Registry/Query": "",
            "Registry/KCBCreate": "",
            "Registry/KCBDelete": "",
            "Registry/Create": "",
            "Registry/DeleteValue": "",
            "Registry/Delete": "",
            "Registry/Flush": "",
            "SystemConfig/Network": "",
            "UdpIp/Send": "",
            "UdpIp/Recv": "",
            "UdpIp/SendIPV6": "",
            "UdpIp/RecvIPV6": "",
            "TcpIp/Send": "",
            "TcpIp/Recv": "",
            "TcpIp/Connect": "",
            "TcpIp/Reconnect": "",
            "TcpIp/Disconnect": "",
            "TcpIp/TCPCopy": "",
            "TcpIp/Retransmit": "",
            "TcpIp/Accept": "",
            "TcpIp/SendIPV6": "",
            "TcpIp/RecvIPV6": "",
        }
        return event_id_mapping.get(event_name, "0")
    
    def println(verbose,msg):
       if(verbose=='1'):
          print(msg)
       else:
          pass
    
class functions_old():
      
    def returnValue(maxper,mpid,verbose):
        s=''
        if(maxper<0.4):
            s = '1000'
        else:    
            s = str(int(maxper*10))+'00'+str(mpid)
        func.println(verbose, s +' maxper = '+ str(int(maxper*10))+' 00 '+ ' mpid = '+ str(mpid))
        return int(s)
                
    def inferTest(csvFile,model,verbose):   #not in use     
        #import dataset
        logs=[]
        logs.append(pd.read_csv(csvFile, on_bad_lines='skip', low_memory=False))#Benign
        log = pd.concat(logs)
        MaliciousProcessIDs = []
        #rowsToImport=1000
        func.println(verbose,log.columns)
        func.println(verbose,log.shape)
        #preprocess Data
        df = functions.preProcessData(log,verbose)
        #load given sample size for predictions
        #if(df.shape[0]>rowsToImport+1):
        #   df = df.sample(rowsToImport)        
        #Label Data
        df['label'] = np.where((df['pid'].isin(MaliciousProcessIDs)), '1', '0')
        func.println(verbose,df['label'].value_counts())  
        #Normalize Data      
        features,labels = functions.normalizeData(df,verbose)
        func.println(verbose,features.shape)
        #Load Model and predict
        func.println(verbose,'Model Name: '+ model)
        import joblib
        model_dt = joblib.load(model)
        y_perd_dt = model_dt.predict(features)        
        y_perd_dt = (model_dt.predict(features) > 0.5).astype("int32")
        func.println(verbose,y_perd_dt.shape)
        df['y_perd_dt']=pd.Series(np.array(y_perd_dt).flatten())
        #check prediction accuracy
        acc_score_dt = accuracy_score(labels, y_perd_dt)
        func.println(verbose,"Accuracy Score for dt: "+ str(acc_score_dt*100))
        #get prediction score if pid is malicious or not        
        predictions = df[['pid','label','y_perd_dt']]
        predictions = predictions.dropna(axis=0)
        func.println(verbose,predictions.groupby(['pid'])['y_perd_dt'].sum())
        func.println(verbose,df.shape[0])
        mpid = predictions.groupby(['pid'])['y_perd_dt'].sum().idxmax()
        res = predictions.groupby(['pid'])['y_perd_dt'].sum()
        maxper = (res.max()/df.shape[0])*100
        func.println(verbose,'PID: ('+ str(mpid) + ') is malicious. Infection rate is. '+ str(maxper)+'%')
        func.println(1, 'maxper='+ str(int(maxper))+' mpid='+ str(mpid)+' count='+ str(int(res.max()))+' total='+ str(df.shape[0]))
        return maxper,mpid
            
    def Tfidf_infer(vocabpath,df,min,max,verbose):#not in use
        func.println(verbose,'TfidfVectorizer')        
        try:
            from sklearn.feature_extraction.text import TfidfTransformer,TfidfVectorizer
            import pickle
            loaded_vec = TfidfVectorizer(analyzer = 'char_wb', ngram_range = (min,max), decode_error="replace",vocabulary=pickle.load(open(vocabpath,"rb")))
            tfidf = TfidfTransformer().fit_transform(loaded_vec.fit_transform(df))
            arr = tfidf.toarray()
            tfidf_tokens =loaded_vec.get_feature_names_out()
            df = pd.DataFrame(arr,columns=tfidf_tokens)
            func.println(verbose,'145')
        except Exception as e:
            func.println(verbose,e)
            func.println(verbose,'error in Tfidf_infer function')   
            return 0         
        return df
    
    def normalizeData_tfidf_train(df1,min,max,Output_CSV_Path,Output_Vocab_Path,verbose): #not in use
        func.println(verbose,'normalizing dataset')
        df_tfidf = functions.Tfidf_train(df1['eid'],min,max,Output_Vocab_Path,verbose)
        func.println(verbose,'131')
        #Sort dataframe wrt ts,pid
        df=df1.sort_values(['ts','pid','pn'])
        #add time diff column i.e time between multiple accees to same [File Type] by a process
        df_tfidf['ts_ft_diff'] = df.groupby(['pid','ft','pn'])['ts'].diff()
        #add time diff column i.e time between multiple accees to [Files] by a process
        df_tfidf['ts_diff'] = df.groupby(['pid','pn'])['ts'].diff()
        df_tfidf['n_eid'] =df['eid'].str.len()
        df_tfidf['pn']=functions.labelEncoder(df,'pn')
        func.println(verbose,'141')
        df_std = functions.standardizeUsingStandardScaler(df_tfidf,verbose)
        func.println(verbose,'143')
        df_std['label'] = df1['label']
        df_std = df_std.fillna(0)
        df_std = df_std.reset_index()
        func.println(verbose,'147')
        # try:
        #     df_std=df_std[[110, 'pn', 124, 5, 'ts_ft_diff', 12, 'ts_diff', 66, 3, 69, 79, 50, 22, 80, 73, 85, 119, 'n_eid', 9, 57,'label']]
        # except Exception as e:
        #     func.println(verbose,e)
        #     func.println(verbose,df_std.shape)
        #     return 0
        df_std=df_std.drop(['index'],axis=1)
        func.println(verbose,df_std.shape)
        func.println(verbose,df_std.columns)
        df_std.to_csv(Output_CSV_Path, index=False)
        return 
    
    def Tfidf_train(df,min,max,Output_Vocab_Path,verbose): #not in use
            from sklearn.feature_extraction.text import TfidfVectorizer
            coun_vect = TfidfVectorizer(analyzer = 'char_wb', ngram_range = (min,max))
            coun_matrix = coun_vect.fit_transform(df)
            import pickle
            pickle.dump(coun_vect.vocabulary_,open(Output_Vocab_Path,"wb"))
            arr = coun_matrix.toarray()
            df = pd.DataFrame(arr)
            #print( coun_vect.get_feature_names_out())
            func.println(verbose,'TfidfVectorizer shape ={}'.format(df.shape))
            return df
      
    def normalizeData(df1,verbose): #not in use
        #remove rows with very rare event patterns < =5
                #value_counts = df1.eid.value_counts()
                #to_remove = value_counts[value_counts <= 5].index
                #df1 = df1[~df1.eid.isin(to_remove)]
        #func.println(verbose,df1.shape)
        #remove rows with events <= 3
            #df1=df1.assign(eid_g3 = np.where(((df1['eid'].str.len()>=3)==True), '1', '0'))
            #indices_toremove = df1[(df1['eid_g3']=='0')].index
            #df2 = df1.drop(indices_toremove)
            #dff =df2.drop('eid_g3',axis=1)
        #func.println(verbose,'Deleted rows: '+ str(len(indices_toremove)))
        #func.println(verbose,'New shape: '+ str(dff.shape))
        #Sort dataframe wrt ts,pid
        df=df1.sort_values(['ts','pid','pn'])
        #add time diff column i.e time between multiple accees to same [File Type] by a process
        df['ts_ft_diff'] = df.groupby(['pid','ft','pn'])['ts'].diff()
        #add time diff column i.e time between multiple accees to [Files] by a process
        df['ts_diff'] = df.groupby(['pid','pn'])['ts'].diff()
        #remove timestamp column
        df.drop('ts',axis=1,inplace=True)

        #df[(df['label']==1) & df['eid'].str.contains("fnncccl|fnnccccl|fnncccccl")==True]
        dt_count = df.groupby('pid').agg({'ft':lambda x: len(pd.unique(x))})
        df = pd.merge(df, dt_count,how='right', on=['pid'])
        df.rename(columns = {'ft_x':'ft','ft_y':'ft_count'}, inplace = True)
        df['n_eid'] =df['eid'].str.len()
        df['create'] = df['eid'].str.count('f')
        #df['nlg'] = np.where((df['eid'].str.contains("nlg")==True), '1', '0')
        df['lgfn'] = np.where((df['eid'].str.contains("lgfn|lgfen")==True), '1', '0')
        df['fnncccl'] = np.where((df['eid'].str.contains("fnncccl|fnnccccl|fnncccccl")==True), '1', '0')
        df['fnblg'] = np.where((df['eid'].str.contains("fnblg")==True), '1', '0')
        df['dnlg'] = np.where((df['eid'].str.contains("dnlg")==True), '1', '0')
        df['elgf'] = np.where((df['eid'].str.contains("elgf")==True), '1', '0')
        df['eid']=functions.labelEncoder(df,'eid')
        df['pn']=functions.labelEncoder(df,'pn')
        #func.println(verbose,df.shape)
        dfx=df.drop('label',axis=1)
        if(dfx.shape[0]<5):
           func.println('1','Dataframe has no rows left to Scale.')
        from sklearn.preprocessing import StandardScaler
        scaler = StandardScaler()
        df_std= pd.DataFrame(scaler.fit_transform(dfx.values), columns = dfx.columns)
        #df_std = standardizeUsingStandardScaler(dfx)
        df_std['label']=df['label']
        df_std = df_std.dropna(axis=0)
        df_std.reset_index()
        #func.println(verbose,df_std.columns)
        #func.println(verbose,df_std.shape)
        #df_std.info(verbose=False,show_counts= True)

        features=df_std.drop(['ppid'],axis=1)
        features=features.drop(['label'],axis=1)

        #func.println(verbose,features.shape)
        #func.println(verbose,features.columns)
        labels=df_std['label'].astype(int)
        return features,labels

    def featureSelection_using_ExtraTreesClassifier(features, labels,top): #not in use
      import pandas as pd
      from sklearn.ensemble import ExtraTreesClassifier
      model=ExtraTreesClassifier(random_state=42)
      model.fit(features, labels)
      feature_imp = pd.Series(model.feature_importances_, index=features.columns)
      print(feature_imp.nlargest(top).index.values)
      return features[feature_imp.nlargest(top).index.values],feature_imp.nlargest(top)

