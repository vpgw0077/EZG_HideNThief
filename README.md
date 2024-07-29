# Hide N Thief
![title](https://github.com/user-attachments/assets/4ead7a90-ce1a-4e67-86c4-af3bc80698bc)

## 개요
- 프로젝트 이름 : Hide N Thief
- 장르 : 탈출, 1인칭, 스릴, 싱글플레이
- 개발 도구 : Unity ver.2019.4.9f1, C#
- 개발 인원 : 4명
- 담당 역할 : 게임 기획, 플레이어, 적AI, 미션 등 핵심 기능 개발, 레벨 디자인

## 구동 방법
프로젝트를 열고 Assets -> Scenes -> Main 씬을 열고 플레이하면 됩니다.

## 프로젝트 설명
- 플레이어는 감옥을 탈출한 도둑이 되어 경찰이 가득한 어두운 숲 속에서 무사히 탈출해야 합니다!
- Hide N Thief는 경찰에게 붙잡히지 않고 아이템을 적절히 활용하여 미션을 수행해야 합니다.

## 게임 플레이 방식
#### 플레이어 조작

|이동|점프|상호작용|손전등 조작|달리기|아이템 사용|아이템 교체|
| :---: |:---:|:---:|:---:|:---:|:---:|:---:|
|WASD| Space |E|F|Left Shift|Left Mouse|1 2 3 4|
- 플레이어는 스태미너를 소모하여 달릴 수 있고 달리지 않으면 자동으로 스태미너가 회복된다.
- 손전등은 앞을 비출뿐만 아니라 아이템 탐지 거리가 상승하고 함정을 탐지할 수가 있다. 손전등을 켜면 배터리를 소모하고 아이템을 획득해 충전할 수 있다.

![_2024_07_29_23_59_16_471-ezgif com-video-to-gif-converter](https://github.com/user-attachments/assets/7020a6d5-17a8-442f-b97a-6800e258af5e)


#### 플레이어 장비

|돌멩이|섬광탄|연막탄|에너지 드링크
|:---:|:---:|:---:|:---:|
|![r](https://github.com/user-attachments/assets/8a82ed7d-027c-41c1-add3-31a555dc7457)|![fb](https://github.com/user-attachments/assets/53b82198-ba14-4bf4-8a3e-cdf124f3123a)|![ss](https://github.com/user-attachments/assets/cdc52bc2-0da3-4102-98a9-eff84a82bf0a)|![ed](https://github.com/user-attachments/assets/f1643f47-f93c-43e0-80f7-25b57ace1169)|
|1번 장비로 던진 위치로<br>경찰을 유인하는<br>효과를 가지고 있음|2번 장비로 범위 내<br>경찰을 기절시킴|3번 장비로 플레이어가<br>숨을 수 있는<br>공간을 만들어줌|4번 장비로 사용 시<br>달릴 때 스태미너를 소모하지 않음|


#### 경찰
- 적 요소로 FSM을 활용해 패턴을 구현했다.
- 패턴 : 수색, 추격, 기절
- 추격 중인 경찰에게 붙잡히면 게임 오버가 되고 일정 거리를 벗어나면 추격을 중단한다.

|![_2024_07_30_00_08_32_218-ezgif com-video-to-gif-converter](https://github.com/user-attachments/assets/6e35042c-5183-48d0-a529-85567631d51a)|![_2024_07_30_00_09_48_15-ezgif com-video-to-gif-converter](https://github.com/user-attachments/assets/4f80e799-d5d7-4aa2-b81e-41e2d05f050b)|![_2024_07_30_00_10_57_539-ezgif com-video-to-gif-converter](https://github.com/user-attachments/assets/c7776cb1-71b0-4b3e-b808-1203b0e4e20b)|
| :---: |:---:|:---:|
|정찰 상태| 플레이어 발견 및<br>추격 시작 |기절 상태|


#### 미션
- 발전기 미션과 기름통 미션이 존재하며 랜덤하게 설정된다.
- 발전기 미션은 맵에 존재하는 3개의 발전기를 가동하는 미션이다.
- 기름통 미션은 맵에 존재하는 3개의 기름통을 획득한 후 탈출구 주변에 있는 발전기를 가동시키는 미션이다.
