# 1911_docomo_AVATAR_ML_Unity


## Versions
- Unity: 2019.2.11f1
- MLSDK: 0.22.0
- LuminOS: 0.97.01


--------------------------------------------------
## MLアプリ起動の流れ
1. アプリ起動
2. 初回マーカー読み込み
3. マネージPCと通信開始（疎通するまでポーリング）
4. 疎通後、待機ダンス開始（Idolステート）


--------------------------------------------------
## MagicLeap IP
ML12台分、下二桁をインデックス追加
```
192.168.1.201
192.168.1.202
192.168.1.203
...
192.168.1.212
```

## MagicLeap PORT
```
5555
```

--------------------------------------------------
## Camera Display IP
```
192.168.1.231
192.168.1.232
192.168.1.233
```

--------------------------------------------------
## 管理アプリ IP/PORT
```
IP: 192.168.1.101
PORT: 5039
```

--------------------------------------------------
## ScannedData IP/PORT
```
IP: 192.168.1.101
PORT: 3000
```


--------------------------------------------------
## 通信信号、送受信区切り文字列（語尾に付与）

```
=====ANATOME_ML_MESSAGE=====
```


--------------------------------------------------

## MagicLeap受信信号（TCP/Json形式）


#### モデル読み込み
※modelsの配列の長さは最小1最大4
```
{
	"models": [
		{
			"id": "フォルダ名",
			"fbx": "fbxのURL",
			"tex": "textureのURL"
		},
		{
			"id": "フォルダ名",
			"fbx": "fbxのURL",
			"tex": "textureのURL"
		},
		{
			"id": "フォルダ名",
			"fbx": "fbxのURL",
			"tex": "textureのURL"
		},
		{
			"id": "フォルダ名",
			"fbx": "fbxのURL",
			"tex": "textureのURL"
		}
	]
}
```

#### ダンス再生
```
{
	"dance": "play",
	"pattern": "0" // ダンスパターンはどれか選択: "0", "1", "2"
}
```



--------------------------------------------------

## MagicLeap送信信号（TCP/Json形式）

#### モデル読み込み完了
※modelsの配列の長さは最小1最大4
```
{
	"device": "display", // display, ml
	"models": [
		{
			"id": "フォルダ名"
			"isDone": boolean, // 読み込み成功したか(fbx and texture)
			"isFbx": boolean, // fbxの読み込み成功したか
			"isTex": boolean // textureの読み込み成功したか
		},
		{
			"id": "フォルダ名"
			"isDone": boolean, // 読み込み成功したか(fbx and texture)
			"isFbx": boolean, // fbxの読み込み成功したか
			"isTex": boolean // textureの読み込み成功したか
		},
		{
			"id": "フォルダ名"
			"isDone": boolean, // 読み込み成功したか(fbx and texture)
			"isFbx": boolean, // fbxの読み込み成功したか
			"isTex": boolean // textureの読み込み成功したか
		},
		{
			"id": "フォルダ名"
			"isDone": boolean, // 読み込み成功したか(fbx and texture)
			"isFbx": boolean, // fbxの読み込み成功したか
			"isTex": boolean // textureの読み込み成功したか
		}
	]
}
```
#### ダンス再生完了
```
{
	"device": "display", // display, ml
	"dance": "ended"
}
```

#### (保留)待機中以外で信号を受けた場合
※待機中（IDLE）以外では、信号を受けても処理はせず現在の状態を返す
```
{
	"device": "display", // display, ml
	"state": "DANCE"
}

// stateは以下のどれかを返す
// ダンス中　　： "DANCE"
// モデル読込中: "LOADING"
```


