# ModuleActivationFramework

## 概要
- `ModuleActivationFramework` (以下、`MoAF`) は、プラグイン機能とDIコンテナの提供により、モジュール間疎結合の設計を可能にしたC# (WinForm アプリケーション) 向けフレームワークである。
- 概要は `.\MoAF\docs\MoAF_フレームワーク仕様書.html` を参照すること。

## 構成
- 直下に以下のディレクトリが存在する。
  - MoAF
  - ThemeParkApp
- `MoAF` は、本フレームワークのプロジェクトである。
- `ThemeParkApp` は、`MoAF` の動作確認および、特徴を示すために作成したアプリケーションプロジェクトである。

## 動作環境
- 本フレームワークは、以下の環境で作成されている。
  - Windows11
  - Visual Studio 2022
  - .Net8.0

## 使い方
### MoAF
- 前提として、`MoAF` は以下の3つのプロジェクトで構成されている。
  - MoAF.Abstractions
  - MoAF.Core
  - MoAF.Container
- `MoAF` は、上記3プロジェクトそれぞれにおいて、nugetパッケージを作成している。
- nugetパッケージは、それぞれ以下に存在する。
  - .\MoAF\MoAF.Abstractions\bin\Release\MoAF.Abstractions.1.0.0.nupkg
  - .\MoAF\MoAF.Core\bin\Release\MoAF.Core.1.0.0.nupkg
  - .\MoAF\MoAF.Container\bin\Release\MoAF.Container.1.0.0.nupkg
- ローカル環境に上記のnugetパッケージを配置し、`MoAF` を使用するプロジェクトにおいて、nugetでインストールすることで使用可能になる。
- 内部設計などは、`.\MoAF\docs\MoAF_全体設計書.html` を参照すること。(詳細な設計書は後日更新予定)
### ThemeParkApp
#### 概要説明
- このアプリケーションは、主要なテーマパークのアトラクション待ち時間を取得するアプリケーションである。`MoAF` の有用性を確認するために作成している。
- `ThemeParkApp` は以下のプロジェクトで構成されている。
  - MainApp
  - Common
  - QueueTimesService
  - TokyoDisneyLand
  - TokyoDisneySea
  - UniversalStudioJapan
- `MainApp` は、このアプリケーションの主要機能 (起動、UI など) を担っている。`MoAF.Container` を参照する。
- `Common` は、すべてのプロジェクトから参照可能であり、`MoAF` の使用に必要な共通データを取り扱う。
- `QueueTimesService` は、テーマパークのアトラクション待ち時間を取得するプロジェクトである。`MoAF.Abstractions` を参照する。
- `TokyoDisneyLand` は、東京ディズニーランドのアトラクション待ち時間を `QueueTimesService` を介して取得するプロジェクトである。`MoAF.Abstractions` を参照する。
- `TokyoDisneySea` は、東京ディズニーシーのアトラクション待ち時間を `QueueTimesService` を介して取得するプロジェクトである。`MoAF.Abstractions` を参照する。
- `UniversalStudioJapan` は、ユニバーサルスタジオジャパンのアトラクション待ち時間を `QueueTimesService` を介して取得するプロジェクトである。`MoAF.Abstractions` を参照する。
- `TokyoDisneyLand.dll`、`TokyoDisneySea.dll`、`UniversalStudioJapan.dll` は、モジュールディレクトリ (`.\ThemeParkApp\bin\Release\net8.0-windows\Modules`) へ追加 / 削除を行うことで、使用 / 未使用を切り替え可能である。この際、ホストアプリケーションである `MainApp` のリビルドは不要である。
- この「ホストアプリケーションのリビルドなしに、機能の追加 / 削除が可能」という動きが、`MoAF` 最大の特徴である。
- 本アプリケーションの設計に関しては、`.\ThemeParkApp\docs\xxx` を参照すること。(現在未作成、後日更新予定)
#### 使い方の説明
- アプリケーションを起動すると、以下のような画面が表示される。
![アプリケーション起動](.\images\起動.png)
- 「テーマパークを選択」横にあるコンボボックスを選択すると、待ち時間を取得可能なテーマパーク名が表示される。 (`TokyoDisneyLand.dll`、`TokyoDisneySea.dll`、`UniversalStudioJapan.dll` の中で、モジュールディレクトリに配置されているDLLに対応するテーマパークのみが表示される)
![テーマパーク選択](.\images\テーマパーク選択.png)
- `TokyoDisneySea.dll` をモジュールディレクトリから除外した場合、コンボボックスは以下のような表示になる。
![ディズニーシー除外](.\images\ディズニーシー除外.png)
- 「待ち時間を取得」ボタンを押下すると、右側のテキストボックスに各アトラクションの待ち時間が表示される。
![待ち時間取得](.\images\待ち時間取得.png)

## トラブルシュート
- nugetパッケージインストール時、インストールに失敗する場合がある。主に、.Netのクラスライブラリプロジェクトにおいて発生しやすく、対処は以下で可能である。
  - 該当プロジェクトのプロパティを表示
  - 「ターゲットOS」を「(なし)」から「Windows」に変更
  ![nugetインストール失敗時](.\images\nugetインストール失敗時.png)

---
## 改定履歴
| 版数 | 改定日 | 改定箇所 | 改定内容 |
| --- | --- | --- | --- |
| 初版 | 2026/04/16 | - | - |