# 2021 09 22 jaeyong

## bug fix

1. 게임을 시작할 때 white가 먼저 End turn을 누르게 되면 정상적인 진행이 되지 않는 버그를 해결하였습니다.   

> NetworkManager에서 게임을 시작할 때 미리 Deactivate를 주는 방향으로 해결했습니다.
비정상적인 행동 또한 큰 버그의 요소라고 생각하기 때문에, 비정상적인 행동을 막았습니다.

2. 기물을 생성할 때 돈이 부족할시에도 버튼이 사라지는 버그를 해결하였습니다.

> Build버튼을 누르게 되면 작동하는 BuildPieceRPC 내의 UIManager.instance.ButtonPanelTrue(); 를 바깥으로 뺐습니다.
함수 내부에서 동작하다보니 return하는 경우 BuildPiece시 버튼이 사라지는 오류가 발생했고, return의 위치의 상관없이 정상적으로 게임이 진행되게 수정하였습니다.

3. SelectedObject가 존재하지 않을 때 방향을 수정하려고 할 때 뜨는 nullReference 에러를 방지하기 위한 예외처리를 진행했습니다.

> 효원님이 작성하신 메소드인 MoveManager 내의 turnSelectedPiece 메소드에 줄을 추가하였습니다.

4. Move가 안되는 bug를 해결했습니다. MoveManager.cs:273-274

> MoveManager 내의 MovablePieceHighlight 메소드 필요없는 줄을 전부 삭제했습니다.
> 초기화 부분에 오류가 있었고, MoveManager Update 부분의 초기화를 모든 함수를 시행한 뒤 초기화하게 변경하였습니다.

5. 타일들의 active상태가 false여도 애니메이션이 작동하는 것을 해결했습니다. 메모리에서 큰 손해를 보고 있었네요!

> GameManager 내 GObject.GetComponent<Animator>().enabled = false;를 이용해서 애니메이션을 컨트롤하였습니다.
> 하지만, 더 생각해보아야하는 것은 initilizeTile()에서 모든 애니메이션 enabled를 false로 바꿔주고 있는데,
> 이미 false인 값에서 false로 다시 한번 중복으로 초기화해주는 느낌의 비용이 쌀 지가 의문이네요.

6. 내 말이 아닌 상대의 piece를 선택하여 방향을(dir) 바꿀 수 있었던 오류를 수정했습니다.

> 종속성 (ButtonPanelFalse 안에 DeactivateDirButton)을 두기보다는, 코드를 한 줄 추가하여 다음주까지 버그가 있는지 확인하겠습니다.
> DirButtonFalse()과 True의 메소드를 UIManager에 추가하였습니다.

7. 이미 타일에 말이 존재하는데에도 그 타일을 선택한 후 Build 버튼을 누르게 되면 사라지는 UI 버그를 수정하였습니다. BuildManager.cs:191

> Build 버튼을 누르면 실행되는 메소드에 예외처리 조건을 더 까다롭게 설정했습니다.

<hr>

## needs

1. 기물 생성시 돈이 부족하다는 것을 표시해주세요. 누르면 안나와서 당황스럽습니다.

> 덧붙이자면 cost가 현재 보드 중앙에 뜨고 있는데, UI layer이기 때문에 Ray를 발사할 시 EventSystem에서 걸러지고 있어 Move가 제대로 되지 않는 현상을 발견했습니다.
> 보드 중앙에 뜨는 cost 얼른 수정이 필요합니다.

2. cost 보이는 곳에 현재 가지고 있는 돈의 동기화가 필요합니다.

3. 기물 Prefab의 Canvas(HP bar) Rotation 에 대한 아이디어 fix가 필요합니다.
> 빈 부모 오브젝트를 만들고, 그 안에 자식으로 canvas와 기물 prefab을 넣는 방법
> rotation하기 전에 자신의 default값을 기억해두고 회전할때마다 default값으로 회전시키는 방법
> ps) rotation constraint를 사용해보았지만 이럴 때 쓰는 것이 아니네요. 적용이 되질 않아요. 자식-부모는 rotation이 항상 같이 가요.

<hr>

**Pull 한 후, 일어날 수 있다고 판단되거나 시뮬레이션을 진행하였을 때 나타나는 버그에 대해 제보해주세요.**

**또한, Scene Outlet 접속에 대한(Insepctor 뷰 연결) 수정도 있어 PR시 오류가 날 수 있으므로 Scene 백업 부탁드립니다.**
