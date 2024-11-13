# 등산 안전 가이드
(2023-2) [가상현실] 기말 프로젝트
<br>
<br>
## 1. 사용 기술
<img src="https://img.shields.io/badge/unity-%23000000.svg?style=for-the-badge&logo=unity&logoColor=white"/> <img src="https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white"/>
<br>Editor version - 2022.3.6f1 LTS<br>
Target Platform - PC / VR
<br>
<br>
## 2. 작품 소개
코로나로 인해 외출과 실내 활동에 제약이 생긴 이후부터 '산스타'와 같은 신조어가 생길 만큼 2030 연령대에서 등산 인구가 크게 증가했습니다. <br>
그러나 이와 더불어 등산 인구 중 구조 대상자에서 2030 연령대는 3번째를 차지할 만큼 부상자 수 또한 많아졌습니다.<br>
이러한 상황을 고려하여 2030 연령대를 대상으로 한 등산 안전 가이드 프로그램을 Unity를 사용해 제작했습니다.<br>
프로젝트는 등산 중 마주할 수 있는 몇몇 부상 상황을 제시하고, 사용자는 안내 메시지를 따라 각 상황에 시뮬레이션을 거쳐 학습합니다.
<br>
<br>
## 3. 기능 설명
### 시작 화면
![image](https://github.com/user-attachments/assets/4ba3fd16-1800-4765-8e29-c5c631cafb56)
위와 같이 4개의 응급 조치 사항을 제시합니다.<br>
각 버튼을 클릭하면 해당 상황에 대한 시뮬레이션을 시작합니다.<br>
<br>
### 시뮬레이션 - 심폐소생술
![image](https://github.com/user-attachments/assets/2bec03c3-0bdc-44ba-93b6-700d5c55a311)
시작 화면에서 심폐소생술 버튼을 클릭해 심폐소생술 교육을 시작합니다.<br>
<br>
![image](https://github.com/user-attachments/assets/ff3c2ae7-3d54-4bcd-a7b1-9cd42e1b90d3)
![image](https://github.com/user-attachments/assets/c6868669-b3fe-48d1-bed7-4e457da15128)
![image](https://github.com/user-attachments/assets/96f5060d-601e-432a-a6a2-768324b074ce)

각 상황에 대한 이벤트 발생과 사용자가 취해야 할 행동을 자막으로 안내합니다.<br>
WASD 혹은 방향키로 이동할 수 있으며 마우스를 움직여 시점을 전환할 수 있습니다.<br>
사용자의 직접적인 움직임을 R키를 일정 시간 이상 누르는 것으로 대체합니다.<br>
R키를 누르면 화면의 중앙에 일정 거리의 Ray를 쏘아 Ray가 충돌하는 물체에 따라 알맞은 인터랙션을 수행합니다.<br>
<br>
![image](https://github.com/user-attachments/assets/b4781717-4662-444b-a9d8-1a0143761f78)
이후 안내되는 메시지와 참고 자료에 따라 시뮬레이션을 진행합니다.<br>
<br>
![image](https://github.com/user-attachments/assets/58eae8c8-8915-44d5-94d5-29edce45d0ca)
해당 시뮬레이션을 완료하면 다시 초기 시작 화면으로 돌아가 다음 시뮬레이션으로 진행할 수 있도록 합니다.
