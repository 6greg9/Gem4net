# Gem4NetDeviceService
## 使用套件
### [secs4net](https://github.com/mkjeff/secs4net)  
- 已有功能
  - HSMS
### SQLite
### EF Core
sqlsugar似乎更優秀, 重點是有singleton, ef core 在code first 比較強, 而且微軟將來會優化?
### Dapper
主要是ProcessProgram部分, 用SqlMapper處理類json結構
## 預計開發功能
- Equipment Processing States	
- Host Initiated S1=F13/F14 Scenario	
- Event Notification	
- On-Line Identification	
- Error Messages	
- Control (Operator Initiated)	
- Establish Communication Additional Capabilities	
- Dynamic Event Report Configuration	
- Variable Data Collection	
- Trace Data Collection
- Status Data Collection	
- Alarm Management	
- Remote Control	
- Equipment Constants	
- Process Program Management	
- Equipment Terminal Services Material Movement	
- Clock	
- Limits Monitoring	
- Spooling	
- Control (Host-Initiated)
## 已完成SECS語句
- [x] S1F1  Are You There
- [x] S1F3  Selected Equipment Status Request
- [x] S1F11 Status Variable Namelist Request
- [X] S1F13 Establish Communications Request
- [X] S1F15 Request OFF-LINE
- [X] S1F17 Request ON-LINE
- [ ] S1F21 Data Variable Namelist Request
- [X] S2F13 Equipment Constant Request
- [X] S2F15 New Equipment Constant Send
- [ ] S2F17 Date and Time Request
- [ ] S2F23 Trace Initialize Send
- [X] S2F25 Loopback Diagnostic Request
- [ ] S2F29 Equipment Constant Namelist Request
- [ ] S2F31 Date and Time Set Request
- [X] S2F33 Define Report
- [X] S2F35 Link Event Report
- [X] S2F37 Enable/Disable Event Report
- [X] S2F41 Host Command Send
- [ ] S6F1  Trace Data Send
- [X] S6F11 Event Report Send
- [ ] S6F13 Annotated Event Report Send
- [ ] S6F15 Event Report Request
- [ ] S6F17 Annotated Event Report Request
- [ ] S6F19 Individual Report Request
- [ ] S5
- [X] S7F17 Delete Process Program Send
- [X] S7F19 Current Process Program Dir Request
- [X] S7F23 Formatted Process Program Send
- [X] S7F25 Formatted Process Program Request
- [X] S10F1 Terminal Request
- [X] S10F3 Terminal Display, Single
## Variable Item Dictionary in SEMI E5 (8.4)
## GEM-Defined Collection Events (9.3)
## App Configs
- CommHostEqpTrigger
## ITRI-like Interface
- [ ] Initialize: 建構式加callback?
- [ ] Close: 外部可 Enable, Disable
- [ ] ProcessMessage: 直接把Secs4Net拿來用...
- [X] UpdateSV: SV,DV,EC混在一起?
- [ ] GetSV
- [X] UpdateEC
- [ ] GetEC
- [X] SendTerminalMessage
- [X] EventReportSend
- [ ] AlarmReportSend
- [X] EnableComm
- [X] DisableComm
- [X] GetCurrentCommState
- [X] OnLineRequest
- [X] OffLine
- [X] OnLineLocal
- [X] OnLineRemote
- [ ] Command: 這個應該有細分空間

