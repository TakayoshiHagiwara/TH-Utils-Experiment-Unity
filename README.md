# TH-Utils-Experiment-Unity<!-- omit in toc -->

<img src="https://img.shields.io/badge/Unity-2021 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">

Unityにおける実験関連のメソッド、ツールを扱います。
心理物理実験などでよく使用する著者自作のツールをまとめます。

# Table Of Contents <!-- omit in toc -->
<details>
<summary>Details</summary>

- [Environment](#environment)
- [Installation](#installation)
  - [Unity Package Manager経由での導入](#unity-package-manager経由での導入)
- [Usage](#usage)
  - [Questionnaire](#questionnaire)
    - [Sample scene](#sample-scene)
    - [Prefabs](#prefabs)
- [Description](#description)
- [References](#references)
- [Troubleshooting](#troubleshooting)
- [Versions](#versions)
- [Author](#author)
- [License](#license)

</details>


# Environment
- Unity 2021 or Later

# Installation
## Unity Package Manager経由での導入
1. Window -> Package Managerを開きます
2. 左上のプラスアイコンをクリックし、「Install package from git URL...」をクリックします
3. このリポジトリURLを入力し、Installをクリックします

# Usage
<!-- -------------------------------------------------- -->
## Questionnaire
ディスプレイ上やVR空間でアンケートに回答するためのツールです。
リッカート尺度とVisual Analog Scaleを用意しています。

**日本語への対応はしていません。日本語のアンケート項目を作成する場合は、TextMeshProに日本語のFont Assetを追加してください。**

### Sample scene
1. SamplesからQuestionnaire SamplesをImportし、サンプルシーン (StandardSample) を開いてください
2. ディスプレイ上で確認する場合はそのまま、VRで確認する場合はHMDのcamera rigを配置し、Main Cameraをオフにしてください
3. 実行後、最初は7段階のリッカート尺度が表示されます
4. キーボードの左右矢印、またはVRコントローラのスティックを左右に倒すと、選択の変更ができます
5. キーボードのEnterキー、またはVRコントローラのトリガーを入力すると、次のアンケートに移ります
6. 4番目、5番目はVASが表示され、キーボードの左右矢印、またはVRコントローラのスティックを左右に倒すと値が変化します
      - キーボードの長押し、またはスティックを長く倒すと、高速に変化します 
7. 5番目のアンケートに回答し終わると、Assets/Resources/Questionnaire直下にCSVファイルで結果が保存されます

---
### Prefabs
| Name | Summary |
| ---- | ---- |
| SampleQuestionnaire | サンプルシーンで配置されていたアンケートのprefab |
| QuestionnaireBase | アンケートを配置するベースとなるprefab |
| LikertScaleQuestionnaire | リッカート尺度のprefab |
| VisualAnalogScaleQuestionnaire | VASのprefab |

---
**基本的な使い方**

1. QuestionnaireBase - Canvas - QuestionnaireRoot直下に、 LikertScaleQuestionnaireかVisualAnalogScaleQuestionnaireのprefabを配置します
2. LikertScaleQuestionnaireの場合は、LikertScaleQuestionnaireコンポーネントの以下の項目を任意で変更してください
      - Question Id: アンケートの識別ID
      - Question Text: 質問文
      - Scale Point Count: 尺度の個数。デフォルトは7段階。
      - Scale Labels: それぞれの尺度のラベル
3. VisualAnalogScaleQuestionnaireの場合は、VisualAnalogScaleQuestionnaireコンポーネントの以下の項目を任意で変更してください
      - Question Id: アンケートの識別ID
      - Question Text: 質問文
      - Step: 入力があった際に値を変更する量
      - Left Label Text: 左側ラベル
      - Right Label Text: 右側ラベル
      - Show Value: スケールの値を表示するかどうか
4. QuestionnaireBase - Managers - QuestionnaireCsvExporterの以下の項目を任意で変更してください
      - Output Directory: CSVを出力するフォルダ。デフォルトでは、Editorの場合はAssets直下、ビルドアプリケーションの場合はXXX_Data直下に保存されます。
      - File Name: ファイル名

- 基本的にQuestionnaireRoot直下にLikertScaleQuestionnaireかVisualAnalogScaleQuestionnaireのprefabを配置するだけです
- prefabの個数を変更すれば、アンケート個数を変更できます
- prefabを配置し、上記のパラメータを任意で調整すれば、そのほかは実行時に自動で調整されます


# Description
(TBD)



# References


# Troubleshooting


# Versions
- [CHANGELOG](/CHANGELOG.md)


# Author
- Takayoshi Hagiwara
    - Nagoya Institute of Technology


# License
- MIT License