using System.Collections.Generic;

[System.Serializable]
public class Sort
{
    public bool sorted { get; set; }
    public bool unsorted { get; set; }
}

[System.Serializable]
public class Pageable
{
    public Sort sort { get; set; }
    public int offset { get; set; }
    public int pageSize { get; set; }
    public int pageNumber { get; set; }
    public bool paged { get; set; }
    public bool unpaged { get; set; }
}

[System.Serializable]
public class Page<T>
{
    public List<T> content { get; set; }
    public Pageable pageable { get; set; }
    public int totalPages { get; set; }
    public int totalElements { get; set; }
    public bool last { get; set; }
    public int size { get; set; }
    public int number { get; set; }
    public int numberOfElements { get; set; }
    public Sort sort { get; set; }
    public bool first { get; set; }
}
