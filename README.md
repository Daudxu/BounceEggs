# BounceEggs

基于 Unity Netcode for GameObjects 的 2D 多人联机游戏。玩家轮流控制角色接鸡蛋，鸡蛋碰到玩家反弹，落入水中则对手得分。

---

## 快速开始

1. 用 Unity 6 打开项目
2. 从 **Bootstrap** 或 **Main** 场景运行
3. 点击 **Host** 创建房间，或 **Client** 加入（默认 127.0.0.1:7777）
4. 本机测试：开两个 Play 实例，一个 Host，一个 Client
5. 2 人连接后自动开始，鸡蛋出现，轮流接球得分

---

## 游戏规则

- **回合制**：Host 先控，鸡蛋碰到玩家后切换回合
- **得分**：鸡蛋落入水面时，当前控球方的对手得分
- **分数**：绿色为 Host，蓝色为 Client

---

## 项目结构

```
Assets/
├── Scripts/
│   ├── Bootstrap.cs              # 启动场景，加载 Main
│   ├── UlManager.cs              # UI 管理，Host/Client 按钮
│   ├── GameManager.cs            # 游戏状态、2 人自动开始
│   ├── PlayerController.cs       # 玩家拖拽移动
│   ├── PlayerColorizer.cs        # 网络同步玩家颜色
│   ├── PlayerStateManager.cs     # 玩家启用/禁用（回合控制）
│   ├── PlayerSelector.cs         # 回合切换、订阅 Egg.onHit
│   ├── Egg.cs                    # 鸡蛋碰撞反弹、onHit/onFallInWater
│   ├── EggManager.cs             # 游戏开始生成鸡蛋
│   ├── ScoreManager.cs           # 得分管理、分数同步
│   ├── ScreenBoundsWall.cs       # 屏幕边界墙
│   ├── ClientNetworkTransform.cs # 客户端权威位置同步
│   └── SceneLoadCleanup.cs       # 移除重复 EventSystem/AudioListener
├── Scenes/
│   ├── Bootstrap.unity           # 入口场景
│   └── Main.unity                # 主游戏场景
└── Mushy Bounce/                 # 美术资源、预制体
```

---

## 核心流程

| 流程 | 说明 |
|------|------|
| 连接 | GameManager 监听 OnClientConnectedCallback，2 人时 StartGame |
| 回合 | PlayerSelector 订阅 Egg.onHit，切换 isHostTurn 并 Enable/Disable 玩家 |
| 得分 | ScoreManager 订阅 Egg.onFallInWater，根据 IsHostTurn 给对手加分 |
| 同步 | UpdateScoreClientRpc 广播分数，UpdateScoreTextClientRpc 更新 UI |

---

## 学习路线：Unity Netcode for GameObjects

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

### 二、RPC 命名约定

- `[ServerRpc]` 方法必须以 `ServerRpc` 结尾
- `[ClientRpc]` 方法必须以 `ClientRpc` 结尾

### 三、创建 LAN 对局

- **主机**：`ServerListenAddress` 设为 `0.0.0.0`（监听所有网卡）
- **客户端**：`Address` 填主机局域网 IP（如 `192.168.1.100`）
- **端口**：默认 7777，可在 Unity Transport 中修改

### 四、使用 Unity Relay 跨地区联机

- 在 Unity Dashboard 创建 Relay 项目
- 配置 `Unity Relay Transport` 或 Relay 模式
- 主机创建 Allocation，获取 JoinCode
- 客户端用 JoinCode 加入，无需知道 IP

### 五、使用 Lobby 进行房间管理与匹配

- 安装 `com.unity.services.lobby`
- 配置 Unity Gaming Services（Authentication、Lobby）
- 主机创建 Lobby 并上传房间信息
- 客户端拉取 Lobby 列表，选择房间后加入

### 六、网络延迟与同步策略

| 策略 | 说明 |
|------|------|
| 客户端权威移动 | `ClientNetworkTransform`，`OnIsServerAuthoritative() = false` |
| 服务端权威 | 默认 `NetworkTransform`，服务端校验位置 |
| 插值 | `NetworkTransform` 内置插值，减少抖动 |
| RPC | 关键逻辑用 `[ServerRpc]` / `[ClientRpc]` 保证一致性 |

---

## 依赖

- Unity 6
- Netcode for GameObjects 2.9.2
- Input System 1.18.0
- TextMeshPro
- URP 2D

---

## 参考资源

- [Netcode for GameObjects 文档](https://docs-multiplayer.unity3d.com/netcode/current/about/)
- [Unity Relay](https://docs.unity.com/relay/)
- [Unity Lobby](https://docs.unity.com/lobby/)
