## 0918

## 주요 내용

**Move 버튼 OnClick Method 적용 완료**

시나리오

1. 기물을 Click
2. Move Button을 Click 
3. 그 기물이 Movable인지 확인
4. 갈 수 있는 Tile들 Highlight 해줌
5. 이동 가능!

**UI 아래의 오브젝트 Ray 불가능하게 적용 완료**

### 수정 세부 내역

- [x] Move Button도 GameScene에 Outlet Connect
- [x] 턴종료 시 Initilize Camera, UI
- [x] Move만을 위한 Ray였던 것을 상태정보 보게 변경 (MoveObject, SelectedObject 추가)
- [x] Move 버튼에 OnClickMove함수 할당
- [x] Tile - OnMouseUp에 !EventSystem.current.IsPointerOverGameObject() 추가
- [x] MoveManager - Ray 쏠 떄 !EventSystem.current.IsPointerOverGameObject() 추가

EventSystem.current.IsPointerOverGameObject() 란?  

UnityEngine.EventSystems 라이브러리의 메소드 중 하나로, Check if the mouse was clicked over a UI element 라고 공식 docs에서 소개하고 있다.

직역하자면 **UI 요소 위에서 마우스를 클릭하였는지 확인하여 UI요소 위였다면 true를 반환**한다. => 따라서 조건문에서 ! (not) 연산자를 사용하여 체크하고 있다.

실제 사용으로는 if를 나누어 사용하지만, 가독성을 위하여 &&로 묶어놓았다.

Tile.cs:29

```cs
private void OnMouseUp()
    {
        int x = (int)gameObject.transform.position.x;
        int z = (int)gameObject.transform.position.z;
        if (GameManager.instance.GetPlayer() && !EventSystem.current.IsPointerOverGameObject())
        {
```

MoveManager.cs:215

```cs
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hit))
            {
```

자세한 설명을 더 듣고싶다면 개인 연락 주세요!

ref) https://docs.unity3d.com/kr/530/ScriptReference/EventSystems.EventSystem.IsPointerOverGameObject.html

# 미처 수정하지 못한 것

**Camera Animation Trigger vs Bool**

