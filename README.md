# BounceEggs

基于 Unity Netcode for GameObjects 的 2D 多人联机游戏学习项目。从零掌握网络多人游戏的完整配置与开发流程。

---

## 学习路线：掌握 Unity Netcode for GameObjects

### 一、完整配置与使用方法

1. **安装依赖**
   - 在 Package Manager 中安装 `com.unity.netcode.gameobjects`
   - 项目使用版本：2.9.2

2. **核心组件**
   - **NetworkManager**：管理主机/客户端、连接、网络对象
   - **NetworkObject**：需同步的物体必须挂载
   - **NetworkTransform**：位置、旋转、缩放同步
   - **Unity Transport**：默认传输层，支持 LAN 与直连

3. **基本流程**
   - `StartHost()`：创建主机（服务器 + 客户端）
   - `StartClient()`：作为客户端加入
   - `NetworkManager.Singleton`：全局单例访问

---

### 二、从零开发 2D 手机多人联机游戏

项目结构：**UI → 玩家控制 → 网络同步 → 完整游戏循环**

| 阶段 | 内容 | 本项目对应 |
|------|------|------------|
| UI | 连接面板、Host/Client 按钮、等待/游戏面板 | `UlManager.cs` |
| 玩家控制 | 输入、移动、边界限制 | `PlayerController.cs` |
| 网络同步 | 玩家位置、动作、状态 | `ClientNetworkTransform`、`NetworkObject` |
| 游戏循环 | 鸡蛋反弹、得分、胜负 | `Egg.cs`、后续扩展 |

---

### 三、玩家移动、动作、状态同步

- **位置同步**：`NetworkTransform` 或 `ClientNetworkTransform`（客户端权威）
- **动作同步**：`NetworkAnimator` 或 RPC 调用
- **状态同步**：`NetworkVariable<T>` 存储并同步变量

---

### 四、创建 LAN 对局

- **主机**：`ServerListenAddress` 设为 `0.0.0.0`（监听所有网卡）
- **客户端**：`Address` 填主机局域网 IP（如 `192.168.1.100`）
- **端口**：默认 7777，可在 Unity Transport 中修改

---

### 五、使用 Unity Relay 跨地区联机

- 在 Unity Dashboard 创建 Relay 项目
- 配置 `Unity Relay Transport` 或 Relay 模式
- 主机创建 Allocation，获取 JoinCode
- 客户端用 JoinCode 加入，无需知道 IP

---

### 六、使用 Lobby 进行房间管理与匹配

- 安装 `com.unity.services.lobby`
- 配置 Unity Gaming Services（Authentication、Lobby）
- 主机创建 Lobby 并上传房间信息
- 客户端拉取 Lobby 列表，选择房间后加入

---

### 七、网络延迟与同步策略

| 策略 | 说明 |
|------|------|
| 客户端权威移动 | `ClientNetworkTransform`，`OnIsServerAuthoritative() = false` |
| 服务端权威 | 默认 `NetworkTransform`，服务端校验位置 |
| 插值 | `NetworkTransform` 内置插值，减少抖动 |
| RPC | 关键逻辑用 `[ServerRpc]` / `[ClientRpc]` 保证一致性 |

---

### 八、游戏系统扩展

- **得分系统**：`NetworkVariable<int>` 存储分数，`[ClientRpc]` 通知 UI
- **玩家选择器**：Lobby 或自定义匹配逻辑
- **胜负逻辑**：服务端判定，通过 RPC 通知客户端
- **动画与音效**：`NetworkAnimator` 同步动画，音效在碰撞/RPC 时本地播放

---

## 项目结构
