# Gem4NetDeviceService
## 使用套件
### [secs4net](https://github.com/mkjeff/secs4net)  
- 已有功能
  - HSMS
### SQLite
### EF Core
sqlsugar似乎更優秀, 重點是有singleton, ef core 在code first 比較強, 而且微軟將來會優化?
## 預計開發功能
- Fundamental
  - State Models 4, 4.3, 4.5
  - Equipment Processing States 4.6
  - Host-Initiated S1,F13/F14 Scenario 5.2.5.1
  - Event Notification 5.3.1.2
  - On-line Identification 5.3.6
  - Error Messages 5.10
  - Control (Operator-Initiated) 5.13 (except 5.13.5.2)
  - Variable data items (GEM, § 8)
  - SECS-II data item restrictions (GEM, § 8)
  - Collection events (GEM, § 9)
- Capability
  - Establish Communications 7.2, 6.4
  - Event Notification 7.3.1.2
  - Dynamic Event Report Configuration 7.3.1.3
  - Data Variable and Collection Event Namelist Requests 7.3.1.4
  - Variable Data Collection 7.3.2
  - Trace Data Collection 7.3.3
  - Limits Monitoring 7.3.4
  - Status Data Collection 7.3.5
  - On-line Identification 7.3.6
  - Alarm Management 7.4
  - Remote Control 7.5
  - Equipment Constants 7.6
  - Process Recipe Management 7.7
  - Material Movement 7.8
  - Equipment Terminal Services 7.9
  - Error Messages 7.10
  - Clock 7.11
  - Spooling 7.12
  - Control (Operator-Initiated) 7.13(except 7.13.5.1)
  - Control (Host-Initiated) 7.13.5.1
## 已完成SECS語句
- [x] S1F3  Selected Equipment Status Request
- [x] S1F11 Status Variable Namelist Request
- [ ] S1F21 Data Variable Namelist Request
