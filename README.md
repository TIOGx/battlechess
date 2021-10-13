<h3 align="center">Battle_Chess_Project</h3>

## Table of Contents

- [게임 장르](#게임-장르)
- [게임 컨셉](#게임-컨셉)
- [사용자 설명서](#게임-규칙-설명)
- [상세 기획](#상세-기획)
- [버그 및 기능 요청](#버그-및-기능-요청)
- [함수 정리](./chess-function.md)
* * *

## 게임 장르
```
보드형 전략적 시뮬레이션 게임
```
## 게임 컨셉
```
체스 말의 이동방식에 파랜드택틱스의 전투 및 턴제 플레이 방식을 접목시킨 2인용 보드형 전략적 시뮬레이션 게임
```
### 게임 규칙 설명
- 게임 진행
  ```
  기본적으로 2인의 플레이어가 각각 체스의 흑, 백 진영을 맡아 번갈아가며 진행하는 Turn based boardgame
  
  각 플레이어의 턴은 build Tower, Move Tower, SelectTowerDir 으로 이루어져 있으며 각 턴이 끝날시 전투 phase가 실행되어 플레이어 타워의 전투가 이루어진다.
  
  플레이어의 비용의 경우 기본적으로 3 ~ 10까지 매턴 1씩 증가하는 형태로 구성된다.
  
  ```
- 보드 구성
  8 x 8 chessboard
 
- 타워 종류
  각 타워는 종족값 + 직업, 공격력, 체력 으로 이루어져있다.
  
  - 종족값 : Chess의 기본적인 말 구성으로 폰, 룩, 나이트, 비숍, 퀸, 킹으로 이루어져 있으며, 기본적인 타워의 능력치와 Move를 담당한다.
  
  - 직업 : 타워의 공격방식(범위)과 추가 능력치 및 특수 능력을 담당한다.
  
  - 공격력 : 적 타워에게 가해지는 데미지
  
  - 체력 : 타워의 체력 
  
- 타워 건설
  - 위치 조건
    자신 진영의 1 ~ 2 row 에 타워를 건설할 수 있다. 특수 케이스로 특정 타워의 주변에 타워를 건설할 수 있다. (스타 파일런 개념)
  
  - 비용 조건
    타워의 종족값과 직업에 따른 비용 계산을 통해 플레이어는 비용을 지불하고 타워를 건설 가능하다.
  
  - 최대 타워 개수 
    추후에 고려 할 예정
   
   - 기본 제공 타워
    추후에 고려 할 예정

- 타워 강화
  ```
  직업에 따라 퀘스트(누적딜, 누적피해, 특정 위치조건, 몇턴 후 각화) 를 두어 퀘스트 완료시 강화되는 형태 (스텟 상승 및 특수공격 or 공격 방식 강화)
  
  폰 종족값의 경우 체스에서 상대 첫 행에 도달시 원하는 말로 변경 가능한데 이부분을 반영하여 직업은 유지하고 원하는 종족값으로 변경하는 방식의 매커니즘 도입
  ```
    
- 자신의 타워 덱 구성
  ```
  게임 시작 전 로비 메뉴에서 자신의 덱을 구성하여 플레이 할 것인지, 만약 그러하다면 급여제도와 같은 제도가 필요한지?
  덱 구성에 있어 제한을 어떻게 둘 것인지.
  ```
- banpick 방식
  게임의 시작에 있어 특정 직업을 하나씩 금지하고 게임에 들어가는 제도를 돌입하는것은 어떠한가?
  
- 전투 phase 데미지 계산 방식
  ```
  각 타워의 경우 자신의 위치와 Dir에 따라 공격 가능한 대상이 존재하면 공격을 실시하고 공격력 만큼의 dmg를 적 타워에게 가함.
  
  Q. 원딜의 경우 일방적인 딜링이 적합하나, 근거리의 경우 턴 종류시 공격한다는 점에서 일반적인 딜링은 부적합할 수 있다. 고로 근딜의 경우 딜교환이 이루어지는 시스템은 어떠한가.
  ```
- 타워 무브
  각 턴종족값에 따라 1회 움직임을 실시할 수 있다.
  Q. 이 부분에 있어서 타워의 개수가 너무 늘어나면 사용자가 피곤할 수 있다. 이부분을 타워 개수 제한으로 해결할지 혹은 코스트를 통해 기물을 움직이는 방식은 어떠한지. 후자 추천

- 승리조건
  적의 King Tower 제거
  만약 타워 건설의 제약등의 설정이 어려워 존버메타가 될 수 있으니 이 부분을 생각하여 특정 턴 까지 왕이 죽지 않으면 누적 점수(사살한 말의 코스트)를 통해 승패를 결정하는 방식은 어떠한가.
  
### 게임 특징
```
체스의 기본적인 말의 움직임을 따와 플레이어들이 쉽게 이해하고 플레이 할 수 있도록 하고 싶으며, 요즘 게임의 경우 사소한 컨트롤에 게임의 승패가 좌지우지되는 것은 알맞지 않기에
자동 전투 시스템을 택했으며, 전략적인 타워의 움직임 만으로 게임을 풀어나간다는 체스자체의 매력적인 부분을 게임에 반영하였다.
```

### 게임 목표
```
적 King 사살 = 승리
```


